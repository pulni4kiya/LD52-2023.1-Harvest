using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardScreen : MonoBehaviour {
	[SerializeField] private List<RewardItem> rewardItems;

	private List<IRewardsProvider> providers;

	private void Init() {
		if (this.providers != null) {
			return;
		}

		this.providers = GameObject.FindObjectsOfType<MonoBehaviour>(true)
			.Select(mono => mono.GetComponent<IRewardsProvider>())
			.Where(obj => obj != null)
			.ToList();

		foreach (var item in this.rewardItems) {
			item.RewardSelected += this.Item_RewardSelected;
		}
	}

	public void ShowRewards() {
		this.Init();

		var rewards = new List<Reward>();
		foreach (var provider in this.providers) {
			rewards.AddRange(provider.GetRewardOptions());
		}
		rewards.Shuffle(UnityRandomGenerator.Instance);

		if (rewards.Count == 0) {
			return;
		}

		if (rewards.Count == 1) {
			rewards[0].Provider.ObtainReward(rewards[0]);
		}

		for (int i = 0; i < rewardItems.Count; i++) {
			if (i < rewards.Count) {
				rewardItems[i].Initialize(rewards[i]);
				rewardItems[i].gameObject.SetActive(true);
			} else {
				rewardItems[i].gameObject.SetActive(false);
			}
		}

		this.gameObject.SetActive(true);

		GameManager.Instance.Pause();
	}

	private void Item_RewardSelected(Reward reward) {
		reward.Provider.ObtainReward(reward);
		this.gameObject.SetActive(false);

		Debug.Log($"Reward obtained: {reward.Title} - {reward.Description} - {reward.CurrentLevel} / {reward.MaxLevel}");

		EventSystem.current.SetSelectedGameObject(null);

		GameManager.Instance.Unpause();
	}
}

public interface IRewardsProvider {
	public List<Reward> GetRewardOptions();
	public void ObtainReward(Reward reward);
}

public class Reward {
	public IRewardsProvider Provider;
	public Sprite Image;
	public string Title;
	public string Description;
	public int CurrentLevel;
	public int MaxLevel;
}
