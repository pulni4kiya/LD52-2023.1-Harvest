using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : WeaponBase {
	private float damage;
	private float size;

	private Reward damageReward;
	private Reward sizeReward;
	private Reward cooldownReward;

	protected override void Trigger() {
		this.InstantiateProjectile(
			new ProjectileSettings() {
				Velocity = Vector3.zero,
				DestroyAfterTime = 0.1f,
				SingleTickHit = true,
				Size = size,
			},
			GameManager.Instance.Player.ProjectileDirection
		);
	}

	protected override void OnProjectileHit(ProjectileHitEventArgs args) {
		this.DealDamageToMonster(args.Monster, this.damage);
	}

	protected override void InitWeaponRewards() {
		this.damageReward = this.CreateReward(5, "Increase damage by 20%");
		this.sizeReward = this.CreateReward(7, "Increase hit area by 15%");
		this.cooldownReward = this.CreateReward(7, "Decrease time between slashes by 10%");
	}

	protected override void RefreshForUpgrades() {
		this.damage = this.baseDamage * (1f + this.damageReward.CurrentLevel * 0.2f);
		this.size = 1f + this.sizeReward.CurrentLevel * 0.15f;
		this.cooldown = this.baseCooldown * (1f - this.cooldownReward.CurrentLevel * 0.1f);
	}
}
