using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour {
	[SerializeField] protected float baseDamage;
	[SerializeField] protected float baseCooldown = 1f;
	[SerializeField] protected Projectile projectilePrefab;

	protected float timeSinceLastTrigger = 0f;

	protected virtual void Start() {

	}

	protected virtual void Update() {
		this.timeSinceLastTrigger += Time.deltaTime;
		if (this.timeSinceLastTrigger > this.baseCooldown) {
			this.timeSinceLastTrigger -= this.baseCooldown;
			this.Trigger();
		}
	}

	protected Projectile InstantiateProjectile(ProjectileSettings settings) {
		var rotation = Quaternion.FromToRotation(Vector3.right, settings.Velocity);
		return this.InstantiateProjectile(settings, GameManager.Instance.Player.Position, rotation);
	}

	protected Projectile InstantiateProjectile(ProjectileSettings settings, Vector2 direction) {
		var rotation = Quaternion.FromToRotation(Vector3.right, direction);
		return this.InstantiateProjectile(settings, GameManager.Instance.Player.Position, rotation);
	}

	protected Projectile InstantiateProjectile(ProjectileSettings settings, Vector3 position, Quaternion rotation) {
		var projectile = GameObject.Instantiate(this.projectilePrefab, position, rotation);
		projectile.Init(settings);
		projectile.OnHit += this.OnProjectileHit;
		GameObject.Destroy(projectile.gameObject, settings.DestroyAfterTime);
		return projectile;
	}

	protected void DealDamageToMonster(MonsterController monster, float damage) {
		monster.TakeDamage(damage);
	}

	protected void DestroyProjectile(Projectile projectile) {
		GameObject.Destroy(projectile.gameObject);
	}

	protected abstract void OnProjectileHit(ProjectileHitEventArgs args);

	protected abstract void Trigger();
}
