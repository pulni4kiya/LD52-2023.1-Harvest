using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponBase {
	private void Start() {
		this.timeBetweenTriggers = 0.5f;
	}

	protected override void Trigger() {
		this.InstantiateProjectile(new ProjectileSettings() {
			Velocity = GameManager.Instance.Player.ProjectileDirection * 5f
		}, false);
	}

	protected override void OnProjectileHit(ProjectileHitEventArgs args) {
		this.DealDamageToMonster(args.Monster, 10f);
		if (args.HitCount >= 2) {
			this.DestroyProjectile(args.Projectile);
		}
	}
}
