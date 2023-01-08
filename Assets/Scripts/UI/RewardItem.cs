using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private TMP_Text titleText;
	[SerializeField] private TMP_Text descriptionText;

	public event Action<Reward> RewardSelected;

	private Reward reward;

	private void Start() {
		this.GetComponent<Button>().onClick.AddListener(OnRewardSelected);	
	}

	public void Initialize(Reward reward) {
		this.reward = reward;
		this.image.sprite = reward.Image;
		this.titleText.text = reward.Title;
		this.descriptionText.text = reward.Description;
	}

	private void OnRewardSelected() {
		this.RewardSelected(this.reward);
	}
}
