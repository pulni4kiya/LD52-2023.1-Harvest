using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
	[SerializeField] private float edgeOfMap;

	private Camera camera;

	private void Start() {
		this.camera = this.GetComponent<Camera>();
	}

	private void LateUpdate() {
		var maxY = this.edgeOfMap - this.camera.orthographicSize;
		var maxX = this.edgeOfMap - this.camera.orthographicSize * ((float)Screen.width / Screen.height);

		var pos = GameManager.Instance.Player.Position;

		this.transform.position = new Vector3(
			Mathf.Clamp(pos.x, -maxX, maxX),
			Mathf.Clamp(pos.y, -maxY, maxY),
			this.transform.position.z
		);
	}
}
