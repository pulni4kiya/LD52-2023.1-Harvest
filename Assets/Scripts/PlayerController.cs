using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
	[Header("References")]
	[SerializeField] private RectTransform healthFill;
	[SerializeField] private Image healthImage;
	[SerializeField] private Camera camera;

	private Rigidbody2D rb;

	private float health = 100f;
	private float maxHealth = 100f;

	private float speed = 2f;

	public Vector2 Direction { get; private set; }

	public Vector2 ProjectileDirection { get; private set; }

	public Vector2 Position {
		get {
			return this.rb.position;
		}
	}

	private void Awake() {
		this.rb = this.GetComponent<Rigidbody2D>();
	}

	private void Start() {

	}

	private void Update() {
		this.Direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		this.ProjectileDirection = this.camera.ScreenToWorldPoint(Input.mousePosition);
		this.UpdateUI();
	}

	private void FixedUpdate() {
		this.rb.velocity = Vector2.ClampMagnitude(this.Direction, 1f) * this.speed;
	}

	private void UpdateUI() {
		var fill = this.health / this.maxHealth;
		this.healthFill.anchorMax = new Vector2(fill, 1f);
		this.healthImage.color = HSBColor.Lerp(Color.red, Color.green, fill);
	}
}
