using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : WeaponBase {
	[SerializeField] private Transform grenadePrefab;
	[SerializeField] private float grenadeSpeed = 2f;

	[Header("References")]
	[SerializeField] private Camera camera;

	protected override void Trigger() {
		StartCoroutine(this.FireGrenade());
	}

	protected override void OnProjectileHit(ProjectileHitEventArgs args) {
		this.DealDamageToMonster(args.Monster, this.baseDamage);
	}

	private IEnumerator FireGrenade() {
		var grenade = GameObject.Instantiate(this.grenadePrefab, GameManager.Instance.Player.Position, Quaternion.identity);

		var start = (Vector2)grenade.position;
		var target = (Vector2)this.camera.ScreenToWorldPoint(Input.mousePosition);
		var controlPoint = new Vector2((start.x + target.x) / 2f, Mathf.Max(start.y, target.y) + 1.5f);

		var distance = (target - controlPoint).magnitude + (controlPoint - start).magnitude;
		var time = distance / this.grenadeSpeed;

		var dt = 0f;
		while (dt < time) {
			dt += Time.deltaTime;
			grenade.position = BezierHelper.CalculatePoint(start, controlPoint, target, dt / time);
			yield return null;
		}

		GameObject.Destroy(grenade.gameObject);

		this.InstantiateProjectile(
			new ProjectileSettings() {
				Velocity = Vector3.zero,
				DestroyAfterTime = 0.4f,
				SingleTickHit = true,
			},
			target,
			Quaternion.identity
		);
	}
}
