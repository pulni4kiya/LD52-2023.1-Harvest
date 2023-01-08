using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour {
	[SerializeField] private AudioSource musicRewards;
	[SerializeField] private AudioSource musicMain;
	[SerializeField] private AudioSource musicBoss;

	[SerializeField] private Toggle soundsToggle;
	[SerializeField] private Toggle musicToggle;

	private bool areRewardsShowing;
	private bool isBossInGame;

	private float musicVolumeMp;

	public int EliteCount { get; set; }

	public void SetBossInGame(bool value) {
		this.isBossInGame = value;
	}

	public void SetRewardsShowing(bool value) {
		this.areRewardsShowing = value;
	}

	private void Start() {
		this.musicMain.volume = 0f;
		this.musicBoss.volume = 0f;
		this.musicRewards.volume = 0f;

		var enableSounds = PlayerPrefs.GetInt("Sounds", 1) == 1;

		AudioManager2D.instance.enableSounds = enableSounds;
		this.musicVolumeMp = PlayerPrefs.GetFloat("Music");

		this.soundsToggle.isOn = enableSounds;
		this.musicToggle.isOn = this.musicVolumeMp > 0.01f;

		this.soundsToggle.onValueChanged.AddListener(value => {
			PlayerPrefs.SetInt("Sounds", value ? 1 : 0);
			AudioManager2D.instance.enableSounds = value;
		});

		this.musicToggle.onValueChanged.AddListener(value => {
			this.musicVolumeMp = value ? 1f : 0f;
			PlayerPrefs.SetFloat("Music", this.musicVolumeMp);
		});
	}

	private void Update() {
		var mainTargetVolume = this.isBossInGame || this.areRewardsShowing || this.EliteCount == 0 ? 0f : 1f;
		mainTargetVolume *= this.musicVolumeMp;
		this.musicMain.volume = Mathf.MoveTowards(this.musicMain.volume, mainTargetVolume, 1f * Time.unscaledDeltaTime);

		var bossTargetVolume = this.isBossInGame ? 1f : 0f;
		bossTargetVolume *= this.musicVolumeMp;
		this.musicBoss.volume = Mathf.MoveTowards(this.musicBoss.volume, bossTargetVolume, 1f * Time.unscaledDeltaTime);

		var rewardsTargetVolume = this.isBossInGame ? 0.5f : 1f;
		rewardsTargetVolume *= this.musicVolumeMp;
		this.musicRewards.volume = Mathf.MoveTowards(this.musicRewards.volume, rewardsTargetVolume, 1f * Time.unscaledDeltaTime);
	}
}
