using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour {
	[SerializeField] private Projectile projectilePrefab;

	protected float timeBetweenTriggers = 1f;
	protected float timeSinceLastTrigger = 0f;

	protected virtual void Start() {

	}

	protected virtual void Update() {
		this.timeSinceLastTrigger += Time.deltaTime;
		if (this.timeSinceLastTrigger > this.timeBetweenTriggers) {
			this.timeSinceLastTrigger -= this.timeBetweenTriggers;
			this.Trigger();
		}
	}

	protected Projectile InstantiateProjectile(ProjectileSettings settings, bool autoOffset) {
		var position = GameManager.Instance.Player.Position;
		if (autoOffset) {
			position += settings.Velocity.normalized * 0.5f;
		}
		var rotation = Quaternion.FromToRotation(Vector3.right, settings.Velocity);
		return this.InstantiateProjectile(settings, position, rotation);
	}

	protected Projectile InstantiateProjectile(ProjectileSettings settings, Vector3 position, Quaternion rotation) {
		var projectile = GameObject.Instantiate(this.projectilePrefab, position, rotation);
		projectile.Init(settings);
		projectile.OnHit += this.OnProjectileHit;
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
