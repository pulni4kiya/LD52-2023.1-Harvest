using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : WeaponBase {
	protected override void Trigger() {
		this.InstantiateProjectile(
			new ProjectileSettings() {
				Velocity = Vector3.zero,
				DestroyAfterTime = 0.2f,
				SingleTickHit = true,
			},
			GameManager.Instance.Player.ProjectileDirection
		);
	}

	protected override void OnProjectileHit(ProjectileHitEventArgs args) {
		this.DealDamageToMonster(args.Monster, this.baseDamage);
	}
}
