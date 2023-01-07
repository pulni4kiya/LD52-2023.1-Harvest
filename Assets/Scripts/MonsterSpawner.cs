using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
	[SerializeField] private MonsterController monsterPrefab;

	[Header("Start")]
	[SerializeField] private Vector2 startTime;
	[SerializeField] private Vector2 startPeriodRange;

	[Header("End")]
	[SerializeField] private Vector2 endTime;
	[SerializeField] private Vector2 endPeriodRange;

	private void Start() {
		StartCoroutine(SpawnMonsters());
	}

	private IEnumerator SpawnMonsters() {
		yield return new WaitForSeconds(this.startTime.AsTime());

		while (true) {
			MonsterSpawnHelper.SpawnMonster(this.monsterPrefab);

			var progress = Mathf.InverseLerp(this.startTime.AsTime(), this.endTime.AsTime(), GameManager.Instance.Time);

			var waitRange = Vector2.Lerp(this.startPeriodRange, this.endPeriodRange, progress);
			var waitTime = UnityRandomGenerator.Instance.NextFloat(waitRange.x, waitRange.y);

			if (GameManager.Instance.Time + waitTime > this.endTime.AsTime()) {
				break;
			}

			yield return new WaitForSeconds(waitTime);
		}
	}
}

public static class MonsterSpawnHelper {
	public static MonsterController SpawnMonster(MonsterController prefab) {
		var position = GameManager.Instance.Player.Position + UnityRandomGenerator.Instance.NextDirection2D() * GameManager.Instance.SpawnDistance;
		var monster = GameObject.Instantiate(prefab, position, Quaternion.identity);
		monster.ScaleForLevel(GameManager.Instance.MonsterLevel);
		return monster;
	}

	public static float AsTime(this Vector2 data) {
		return data.x * 60f + data.y;
	}
}