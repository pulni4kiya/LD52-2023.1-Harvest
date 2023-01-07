using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponBase {
	protected override void Trigger() {
		this.InstantiateProjectile(new ProjectileSettings() {
			Velocity = GameManager.Instance.Player.ProjectileDirection * 5f,
			DestroyAfterTime = 3f
		});
	}

	protected override void OnProjectileHit(ProjectileHitEventArgs args) {
		this.DealDamageToMonster(args.Monster, this.baseDamage);
		if (args.HitCount >= 2) {
			this.DestroyProjectile(args.Projectile);
		}
	}
}
