using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
	private const float TimeBetweenTakingHits = 0.25f;

	[SerializeField] private float speed = 2f;

	[Header("References")]
	[SerializeField] private RectTransform healthFill;
	[SerializeField] private Image healthImage;
	[SerializeField] private Camera camera;
	[SerializeField] private SpriteRenderer spriteRenderer;

	private Rigidbody2D rb;

	private float health = 100f;

	private float maxHealth = 100f;

	private List<MonsterController> monstersAttackingPlayer = new List<MonsterController>(20);

	public Vector2 Direction { get; private set; }

	public Vector2 ProjectileDirection { get; private set; }

	public Vector2 Position {
		get {
			return this.transform.position;
		}
	}

	public bool IsAlive => this.health > 0f;

	public void FillHealth() {
		this.health = this.maxHealth;
	}

	private void Awake() {
		this.rb = this.GetComponent<Rigidbody2D>();
	}

	private void Start() {
		StartCoroutine(this.PeriodicTakeDamage());
	}

	private void Update() {
		this.Direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		this.ProjectileDirection = ((Vector2)this.camera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)this.transform.position).normalized;
		this.spriteRenderer.flipX = this.ProjectileDirection.x < 0f;
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

	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.isTrigger == false) {
			return;
		}

		var monster = collider.GetComponent<MonsterController>();
		if (monster != null) {
			this.TakeDamage(monster);
			this.monstersAttackingPlayer.Add(monster);
			//Debug.Log($"Damaging monsters: {this.monstersAttackingPlayer.Count}");
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (collider.isTrigger == false) {
			return;
		}

		var monster = collider.GetComponent<MonsterController>();
		if (monster != null) {
			this.monstersAttackingPlayer.Remove(monster);
			//Debug.Log($"Damaging monsters: {this.monstersAttackingPlayer.Count}");
		}
	}

	private void TakeDamage(MonsterController monster) {
		this.health -= monster.DamagePerSecond * TimeBetweenTakingHits;
		this.health = Mathf.Clamp(this.health, 0f, this.maxHealth);
	}

	private IEnumerator PeriodicTakeDamage() {
		var wait = new WaitForSeconds(TimeBetweenTakingHits);
		while (true) {
			foreach (var monster in this.monstersAttackingPlayer) {
				this.TakeDamage(monster);
			}
			yield return wait;
		}
	}
}
