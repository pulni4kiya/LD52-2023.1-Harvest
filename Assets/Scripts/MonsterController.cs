using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterController : MonoBehaviour {
	[SerializeField] private float baseSpeed = 1f;
	[SerializeField] private float baseDamage = 1f;
	[SerializeField] private float baseHealth = 10f;
	[SerializeField] private MonsterType type;
	[SerializeField] private MonsterController bossSpawnee;

	[Header("References")]
	[SerializeField] private Transform visual;

	private Rigidbody2D rb;
	private PlayerController player;

	private float maxHealth;
	private float currentHealth;

	private int xp;

	public float DamagePerSecond { get; internal set; }

	private void Start() {
		this.rb = this.GetComponent<Rigidbody2D>();
		this.player = GameManager.Instance.Player;

		if (this.type == MonsterType.Elite) {
			GameManager.Instance.MusicManager.EliteCount++;
		}
		if (this.type == MonsterType.Boss) {
			GameManager.Instance.MusicManager.SetBossInGame(true);
			this.StartCoroutine(this.DoBossStuff());
		}
	}

	private IEnumerator DoBossStuff() {
		while (true) {
			yield return new WaitForSeconds(30f);
			for (int i = 0; i < 3; i++) {
				MonsterSpawnHelper.SpawnMonster(this.bossSpawnee);
			}
		}
	}

	private void FixedUpdate() {
		this.rb.velocity = (this.player.Position - this.rb.position).normalized * this.baseSpeed;
	}

	private void Update() {
		this.visual.rotation = Quaternion.FromToRotation(Vector3.right, GameManager.Instance.Player.Position - (Vector2)this.transform.position);
	}

	public void ScaleForLevel(int level) {
		this.DamagePerSecond = this.baseDamage + level / 5f;
		this.maxHealth = this.baseHealth * (1f + (level - 1) / 10f) + (level - 1) * 2f;
		this.xp = 1;

		if (this.type == MonsterType.Elite) {
			this.maxHealth *= 2f + (level - 1) * 0.2f;
			this.xp = 5;
			var effect = UnityRandomGenerator.Instance.Next(0, 2);
			if (effect == 0) {
				this.DamagePerSecond *= 2f;
			} else {
				this.baseSpeed *= 2f;
			}
		}

		if (this.type == MonsterType.Boss) {
			this.xp = 500;
			this.maxHealth *= 5f;
			this.DamagePerSecond *= 1.5f;
		}

		this.currentHealth = this.maxHealth;
	}

	public void TakeDamage(float damage) {
		this.currentHealth -= damage;
		this.currentHealth = Mathf.Clamp(this.currentHealth, 0f, this.maxHealth);
		if (this.currentHealth <= 0f) {
			GameObject.Destroy(this.gameObject);
			GameManager.Instance.LevelController.IncreasePlayerXp(this.xp);
			GameManager.Instance.BrainsHarvested++;

			if (this.type == MonsterType.Elite) {
				GameManager.Instance.MusicManager.EliteCount--;
			}
			if (this.type == MonsterType.Boss) {
				GameManager.Instance.MusicManager.SetBossInGame(false);
			}
		}
	}
}

public enum MonsterType {
	Normal,
	Elite,
	Boss,
}
