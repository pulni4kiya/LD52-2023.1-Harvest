using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : WeaponBase {
	[SerializeField] private Transform grenadePrefab;
	[SerializeField] private float grenadeSpeed = 2f;
	[SerializeField] private AudioClip explosionSound;

	[Header("References")]
	[SerializeField] private Camera camera;

	private float damage;
	private float size;

	private Reward damageReward;
	private Reward sizeReward;
	private Reward cooldownReward;

	protected override void Trigger() {
		this.PlayProjectileSound();
		StartCoroutine(this.FireGrenade());
	}

	protected override void OnProjectileHit(ProjectileHitEventArgs args) {
		this.DealDamageToMonster(args.Monster, this.damage);
	}

	private IEnumerator FireGrenade() {
		var grenade = GameObject.Instantiate(this.grenadePrefab, GameManager.Instance.Player.Position, Quaternion.identity);

		var start = (Vector2)grenade.position;
		var target = (Vector2)this.camera.ScreenToWorldPoint(Input.mousePosition);
		var controlPoint = new Vector2((start.x + target.x) / 2f, Mathf.Max(start.y, target.y) + 1.5f);

		var distance = (target - controlPoint).magnitude + (controlPoint - start).magnitude;
		var time = distance / this.grenadeSpeed;

		var dt = 0f;
		var lastPosition = start;
		while (dt < time) {
			dt += Time.deltaTime;
			var newPosition = BezierHelper.CalculatePoint(start, controlPoint, target, dt / time);
			grenade.position = newPosition;
			grenade.rotation = Quaternion.FromToRotation(Vector3.right, newPosition - lastPosition);
			lastPosition = newPosition;
			yield return null;
		}

		GameObject.Destroy(grenade.gameObject);

		AudioManager2D.instance.PlaySoundWithVariation(this.explosionSound);
		this.InstantiateProjectile(
			new ProjectileSettings() {
				Velocity = Vector3.zero,
				DestroyAfterTime = 0.4f,
				SingleTickHit = true,
				Size = this.size
			},
			target,
			Quaternion.identity
		);
	}

	protected override void InitWeaponRewards() {
		this.damageReward = this.CreateReward(5, "Increase damage by 20%");
		this.sizeReward = this.CreateReward(7, "Increase hit area by 15%");
		this.cooldownReward = this.CreateReward(7, "Decrease reload time by 10%");
	}

	protected override void RefreshForUpgrades() {
		this.damage = this.baseDamage * (1f + this.damageReward.CurrentLevel * 0.2f);
		this.size = 1f + this.sizeReward.CurrentLevel * 0.15f;
		this.cooldown = this.baseCooldown * (1f - this.cooldownReward.CurrentLevel * 0.1f);
	}
}
