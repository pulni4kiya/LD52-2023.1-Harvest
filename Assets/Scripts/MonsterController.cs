using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterController : MonoBehaviour {
	[SerializeField] private float baseSpeed = 1f;
	[SerializeField] private float baseDamage = 1f;
	[SerializeField] private float baseHealth = 10f;

	private Rigidbody2D rb;
	private PlayerController player;

	private float maxHealth;
	private float currentHealth;

	private int xp;

	public float DamagePerSecond { get; internal set; }

	private void Start() {
		this.rb = this.GetComponent<Rigidbody2D>();
		this.player = GameManager.Instance.Player;
	}

	private void FixedUpdate() {
		this.rb.velocity = (this.player.Position - this.rb.position).normalized * this.baseSpeed;
	}

	public void ScaleForLevel(int level) {
		this.DamagePerSecond = this.baseDamage + level / 5f;
		this.maxHealth = this.baseHealth * (1f + (level - 1) / 10f) + (level - 1) * 2f;
		this.currentHealth = this.maxHealth;
		this.xp = 1;
	}

	public void TakeDamage(float damage) {
		this.currentHealth -= damage;
		this.currentHealth = Mathf.Clamp(this.currentHealth, 0f, this.maxHealth);
		if (this.currentHealth <= 0f) {
			GameObject.Destroy(this.gameObject);
			GameManager.Instance.LevelController.IncreasePlayerXp(this.xp);
		}
	}
}
