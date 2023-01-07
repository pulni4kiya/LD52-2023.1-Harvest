using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaanger : MonoBehaviour {
	public static GameMaanger Instance;

	public PlayerController Player;


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
