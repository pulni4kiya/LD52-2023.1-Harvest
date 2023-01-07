using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class RoadHelper : MonoBehaviour
{
#if UNITY_EDITOR
	private EdgeCollider2D edgeCollider;
	private LineRenderer lineRenderer;

	private void Awake() {
		this.edgeCollider = this.GetComponent<EdgeCollider2D>();
		this.edgeCollider.isTrigger = true;
		this.lineRenderer = this.GetComponent<LineRenderer>();
	}

	private void Update()
    {
		if (Application.isPlaying == false) {
			this.lineRenderer.SetPositions(this.edgeCollider.points.Select(p => (Vector3)p).ToArray());
			this.lineRenderer.positionCount = this.edgeCollider.pointCount;
		}
    }
#else
	private void Awake() {
		GameObject.Destroy(this.GetComponent<EdgeCollider2D>());
		GameObject.Destroy(this);
	}
#endif
}
