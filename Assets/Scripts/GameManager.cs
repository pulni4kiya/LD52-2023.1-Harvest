using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTime = UnityEngine.Time;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;

	[Header("References")]
	public PlayerController Player;
	public PlayerLevelController LevelController;
	public MusicManager MusicManager;
	public List<Vector3> PlayerSpawnPoints;

	public float Time => UnityEngine.Time.timeSinceLevelLoad;

	public int MonsterLevel => Mathf.CeilToInt(this.Time / 60f);

	[field: SerializeField] public float SpawnDistance { get; private set; }

	public bool IsPaused { get; private set; }

	public int BrainsHarvested { get; set; }

	private void Awake() {
		Instance = this;
		UnityEngine.Time.timeScale = 1f;

		this.Player.transform.position = this.PlayerSpawnPoints[UnityRandomGenerator.Instance.Next(0, this.PlayerSpawnPoints.Count)];
	}

	private void OnDestroy() {
		Instance = null;
	}

	public void Pause() {
		this.IsPaused = true;
		UTime.timeScale = 0f;
	}

	public void Unpause() {
		this.IsPaused = false;
		UTime.timeScale = 1f;
	}
}
