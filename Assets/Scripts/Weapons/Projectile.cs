using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
	private Rigidbody2D rb;
	private ProjectileSettings settings;
	private int hitCount;

	public event Action<ProjectileHitEventArgs> OnHit;

	public void Init(ProjectileSettings settings) {
		this.rb = this.GetComponent<Rigidbody2D>();
		this.rb.velocity = settings.Velocity;
		this.settings = settings;
		this.hitCount = 0;

		if (settings.SingleTickHit) {
			StartCoroutine(DisableColliderAfterOneTick());
		}
	}

	private IEnumerator DisableColliderAfterOneTick() {
		var collider = this.GetComponent<Collider2D>();
		collider.enabled = true;
		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate(); // Counting - hard.
		collider.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.isTrigger == false) {
			return;
		}

		var monster = collider.GetComponent<MonsterController>();
		if (monster != null) {
			this.hitCount++;
			this.OnHit(new ProjectileHitEventArgs() {
				Projectile = this,
				Monster = monster,
				HitCount = this.hitCount
			});
		}
	}
}

public struct ProjectileSettings {
	public Vector2 Velocity;
	public float DestroyAfterTime;
	public bool SingleTickHit;
}

public struct ProjectileHitEventArgs {
	public Projectile Projectile;
	public MonsterController Monster;
	public int HitCount;
}
