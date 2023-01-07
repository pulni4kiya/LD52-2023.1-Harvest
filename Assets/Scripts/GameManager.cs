using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;

	[Header("References")]
	public PlayerController Player;

	public float Time => UnityEngine.Time.timeSinceLevelLoad;

	public int MonsterLevel => Mathf.CeilToInt(this.Time / 60f);

	[field: SerializeField] public float SpawnDistance { get; private set; }

	private void Awake() {
		Instance = this;
	}

	private void OnDestroy() {
		Instance = null;
	}

	private void Start() {

	}

	private void Update() {

	}
}
