using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
	[SerializeField] private MonsterController monsterPrefab;

	[Header("Start")]
	[SerializeField] private Vector2 startTime;
	[SerializeField] private Vector2 startPeriodRange;
	[SerializeField] private int startSpawnCount = 1;

	[Header("End")]
	[SerializeField] private Vector2 endTime;
	[SerializeField] private Vector2 endPeriodRange;
	[SerializeField] private int endSpawnCount = 1;
	[SerializeField] private bool dontStopSpawning;

	private void Start() {
		StartCoroutine(SpawnMonsters());
	}

	private IEnumerator SpawnMonsters() {
		yield return new WaitForSeconds(this.startTime.AsTime());

		while (true) {
			var progress = Mathf.InverseLerp(this.startTime.AsTime(), this.endTime.AsTime(), GameManager.Instance.Time);

			var spawnCount = Mathf.RoundToInt(Mathf.LerpUnclamped(this.startSpawnCount, this.endSpawnCount, progress));
			for (int i = 0; i < spawnCount; i++) {
				MonsterSpawnHelper.SpawnMonster(this.monsterPrefab);
			}

			var waitRange = Vector2.Lerp(this.startPeriodRange, this.endPeriodRange, progress);
			var waitTime = UnityRandomGenerator.Instance.NextFloat(waitRange.x, waitRange.y);

			if (this.dontStopSpawning == false && GameManager.Instance.Time + waitTime > this.endTime.AsTime()) {
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