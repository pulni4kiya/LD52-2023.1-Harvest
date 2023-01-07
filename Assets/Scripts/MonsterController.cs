using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterController : MonoBehaviour {
	[SerializeField] private float baseSpeed = 1f;
	[SerializeField] private float baseDamage = 1f;

	private Rigidbody2D rb;
	private PlayerController player;

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
	}
}
