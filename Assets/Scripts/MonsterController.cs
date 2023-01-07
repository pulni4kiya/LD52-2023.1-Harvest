using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterController : MonoBehaviour {
	[SerializeField] private float baseSpeed = 1f;

	private Rigidbody2D rb;
	private PlayerController player;

	private void Start() {
		this.rb = this.GetComponent<Rigidbody2D>();
		this.player = GameManager.Instance.Player;
	}

	private void FixedUpdate() {
		this.rb.velocity = (this.player.Position - this.rb.position).normalized * this.baseSpeed;
	}

	public void ScaleForLevel(int level) {

	}
}
