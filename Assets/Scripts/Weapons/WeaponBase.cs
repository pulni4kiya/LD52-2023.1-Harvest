using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IRewardsProvider {
	[SerializeField] protected float baseDamage;
	[SerializeField] protected float baseCooldown = 1f;
	[SerializeField] protected Projectile projectilePrefab;
	[SerializeField] protected string displayName;
	[SerializeField] protected string description;
	[SerializeField] protected Sprite image;

	protected float cooldown;

	private float timeSinceLastTrigger = 0f;

	private List<Reward> rewards = new List<Reward>(10);
	private List<Reward> unlockRewardCache;
	private Reward unlockReward;

	protected virtual void Start() {
		this.RefreshForUpgrades();
	}

	protected virtual void Update() {
		this.timeSinceLastTrigger += Time.deltaTime;
		if (this.timeSinceLastTrigger > this.cooldown) {
			this.timeSinceLastTrigger -= this.cooldown;
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

	public List<Reward> GetRewardOptions() {
		this.InitRewards();
		if (this.unlockReward.CurrentLevel == 0) {
			return this.unlockRewardCache;
		} else {
			return this.rewards;
		}
	}

	public void ObtainReward(Reward reward) {
		reward.CurrentLevel++;
		if (reward == this.unlockReward) {
			this.gameObject.SetActive(true);
		}
		this.RefreshForUpgrades();
	}

	private void InitRewards() {
		if (this.unlockReward != null) {
			return;
		}

		this.unlockReward = new Reward() {
			CurrentLevel = 0,
			MaxLevel = 1,
			Title = this.displayName,
			Description = this.description,
			Image = this.image,
			Provider = this,
		};
		this.unlockRewardCache = new List<Reward>() { this.unlockReward };

		this.InitWeaponRewards();
	}

	protected Reward CreateReward(int maxLevel, string description) {
		var reward = new Reward() {
			CurrentLevel = 0,
			MaxLevel = maxLevel,
			Title = this.displayName,
			Description = description,
			Image = this.image,
			Provider = this
		};
		this.rewards.Add(reward);
		return reward;
	}

	protected abstract void OnProjectileHit(ProjectileHitEventArgs args);

	protected abstract void Trigger();

	protected abstract void InitWeaponRewards();

	protected abstract void RefreshForUpgrades();
}
