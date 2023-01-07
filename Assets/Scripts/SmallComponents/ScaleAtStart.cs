using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAtStart : MonoBehaviour
{
	[SerializeField] private float minScale;
	[SerializeField] private float maxScale;

	private void Start()
    {
		this.transform.localScale = Vector3.one * UnityRandomGenerator.Instance.NextFloat(this.minScale, this.maxScale);
    }
}
