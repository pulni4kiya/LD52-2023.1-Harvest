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

	public float Time => UnityEngine.Time.timeSinceLevelLoad;

	public int MonsterLevel => Mathf.CeilToInt(this.Time / 60f);

	[field: SerializeField] public float SpawnDistance { get; private set; }

	public bool IsPaused { get; private set; }

	private void Awake() {
		Instance = this;
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
