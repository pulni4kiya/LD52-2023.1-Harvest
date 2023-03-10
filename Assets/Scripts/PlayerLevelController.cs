using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevelController : MonoBehaviour {
	[Header("References")]
	[SerializeField] private RectTransform levelBarFill;
	[SerializeField] private TMP_Text levelText;
	[SerializeField] private RewardScreen rewardsScreen;

	[Header("Data")]
	[SerializeField] private List<int> xpPerLevel;

	private int playerXp;
	private int playerLevel = 1;

	private int NextLevelIndex => this.playerLevel - 1;

	private bool HasMoreLevels => this.NextLevelIndex < this.xpPerLevel.Count;

	private int NextLevelXp => this.xpPerLevel[this.NextLevelIndex];

	public int PlayerLevel => this.playerLevel;

	private void Start() {
		this.UpdateUI();
		this.rewardsScreen.ShowRewards();
	}

	public void IncreasePlayerXp(int xp) {
		if (this.HasMoreLevels == false) {
			// Max level
			return;
		}

		this.playerXp += xp;
		var nextLevelXp = this.NextLevelXp;
		if (this.playerXp >= nextLevelXp) {
			this.playerXp -= nextLevelXp;
			this.playerLevel++;
			GameManager.Instance.Player.FillHealth();
			this.ShowLevelUpOptions();
		}
		this.UpdateUI();
	}

	private void UpdateUI() {
		this.levelText.text = $"Level {this.playerLevel}";
		this.levelBarFill.anchorMax = new Vector2(this.HasMoreLevels ? (float)this.playerXp / this.NextLevelXp : 1f , 1f);
	}

	private void ShowLevelUpOptions() {
		this.rewardsScreen.ShowRewards();
	}
}
