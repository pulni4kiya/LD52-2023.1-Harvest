using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponBase {
	private float damage;
	private int pierce = 0;

	private Reward damageReward;
	private Reward pierceReward;
	private Reward cooldownReward;

	protected override void Trigger() {
		this.InstantiateProjectile(new ProjectileSettings() {
			Velocity = GameManager.Instance.Player.ProjectileDirection * 20f,
			DestroyAfterTime = 3f,
			Size = 1f,
			SingleTickHit = false
		});
	}

	protected override void OnProjectileHit(ProjectileHitEventArgs args) {
		this.DealDamageToMonster(args.Monster, this.damage);
		if (args.HitCount > this.pierce) {
			this.DestroyProjectile(args.Projectile);
		}
	}

	protected override void InitWeaponRewards() {
		this.damageReward = this.CreateReward(5, "Increase damage by 20%");
		this.pierceReward = this.CreateReward(5, "Pierce monsters +1");
		this.cooldownReward = this.CreateReward(7, "Decrease reload time by 10%");
	}

	protected override void RefreshForUpgrades() {
		this.damage = this.baseDamage * (1f + this.damageReward.CurrentLevel * 0.2f);
		this.pierce = this.pierceReward.CurrentLevel;
		this.cooldown = this.baseCooldown * (1f - this.cooldownReward.CurrentLevel * 0.1f);
	}
}
