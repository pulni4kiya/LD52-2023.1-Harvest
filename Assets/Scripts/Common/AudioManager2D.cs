using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;

public class AudioManager2D : MonoBehaviour
{
	public static AudioManager2D instance;

	public AudioMixerGroup masterMixerGroup;

	public AudioMixerGroup soundsMixerGroup;
	public AudioMixerGroup musicMixerGroup;

	public int initialAudioSources = 10;

	public bool affectedByTimeScale = true;

	public bool enableSounds = true;

	private bool enableMusic = true;

	private float targetMusicVolume = 1f;

	public bool EnableMusic {
		get {
			return this.enableMusic;
		}
		set {
			this.enableMusic = value;
			var mute = value == false;
			for (int i = 0; i < this.musicSources.Count; i++) {
				this.musicSources[i].source.mute = mute;
			}
		}
	}

	private List<AudioSource> soundSources;

	private float lastTimeScale = 1f;

	private float fadeTimeTotal;
	private float fadeTimeCurrent;
	private List<MusicHelper> musicSources;

	void Awake ()
	{
		if (instance != null) {
			GameObject.Destroy (this.gameObject);
			return;
		}

		GameObject.DontDestroyOnLoad (this.gameObject);

		instance = this;

		this.soundSources = new List<AudioSource> (this.initialAudioSources);
		for (int i = 0; i < this.initialAudioSources; i++) {
			var audioSource = gameObject.AddComponent<AudioSource> ();
			audioSource.playOnAwake = false;
			audioSource.outputAudioMixerGroup = this.soundsMixerGroup;
			this.soundSources.Add (audioSource);
		}

		this.musicSources = new List<MusicHelper>(4);
		for (int i = 0; i < 4; i++) {
			var audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.outputAudioMixerGroup = this.musicMixerGroup;
			var helper = new MusicHelper();
			helper.source = audioSource;
			this.musicSources.Add(helper);
		}
	}

	void Update ()
	{
		if (this.affectedByTimeScale == true && Mathf.Abs (this.lastTimeScale - Time.timeScale) > 0.01f) {
			var ts = (Time.timeScale > 0.1) ? Time.timeScale : 1f;
			this.lastTimeScale = ts;

			for (int i = 0; i < this.soundSources.Count; i++) {
				this.soundSources [i].pitch = this.lastTimeScale;
			}
			for (int i = 0; i < this.musicSources.Count; i++) {
				this.musicSources[i].source.pitch = this.lastTimeScale;
			}
		}

		if (this.fadeTimeCurrent < this.fadeTimeTotal) {
			this.fadeTimeCurrent += Time.unscaledDeltaTime;

			foreach (var helper in this.musicSources) {
				helper.source.volume = Mathf.Lerp(helper.initialVolume, helper.targetVolume, this.fadeTimeCurrent / this.fadeTimeTotal);
				if (helper.targetVolume < 0.001f && helper.source.volume < 0.001f) {
					helper.source.Stop();
				}
			}
		}
	}

	public void PlaySoundWithVariation(AudioClip sound, AudioMixerGroup mixerGroup = null, float volumeScale = 1f) {
		if (this.enableSounds == false) {
			return;
		}

		AudioSource audioSource = this.GetAudioSourceForSound(mixerGroup);

		audioSource.volume = UnityEngine.Random.Range(0.85f, 1f);
		audioSource.pitch = UnityEngine.Random.Range(0.85f, 1.15f) * this.lastTimeScale;

		audioSource.PlayOneShot(sound, volumeScale);
	}

	public void PlaySound (AudioClip sound, AudioMixerGroup mixerGroup = null, float volumeScale = 1f)
	{	
		if (this.enableSounds == false) {
			return;
		}

		AudioSource audioSource = this.GetAudioSourceForSound(mixerGroup);

		audioSource.volume = 1f;
		audioSource.pitch = 1f * this.lastTimeScale;

		audioSource.PlayOneShot (sound, volumeScale);
	}

	private AudioSource GetAudioSourceForSound(AudioMixerGroup mixerGroup = null) {
		AudioSource audioSource = null;
		for (int i = 0; i < soundSources.Count; i++) {
			if (this.soundSources[i].isPlaying == false) {
				audioSource = this.soundSources[i];
				break;
			}
		}

		if (audioSource == null) {
			audioSource = this.gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			this.soundSources.Add(audioSource);
		}

		if (mixerGroup == null) {
			mixerGroup = this.soundsMixerGroup;
		}
		audioSource.outputAudioMixerGroup = mixerGroup;

		return audioSource;
	}

	private MusicHelper GetFreeMusicHelper() {
		MusicHelper result = null;

		foreach (var helper in this.musicSources) {
			if (helper.source.isPlaying == false) {
				result = helper;
				break;
			}
		}

		if (result == null) {
			var audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.outputAudioMixerGroup = this.musicMixerGroup;

			var helper = new MusicHelper();
			helper.source = audioSource;

			this.musicSources.Add(helper);

			result = helper;
		}

		return result;
	}

	public void PlayMusic(AudioClip music, bool isLoop = false, float fadeTime = 1f, float startTime = 0f, float volume = 1f) {
		foreach (var helper in this.musicSources) {
			helper.initialVolume = helper.source.volume;
			helper.targetVolume = 0f;
		}

		var musicHelper = this.GetFreeMusicHelper();
		musicHelper.initialVolume = 0f;
		musicHelper.targetVolume = volume;
		
		musicHelper.source.clip = music;
		musicHelper.source.time = startTime;
		musicHelper.source.loop = isLoop;
		musicHelper.source.Play();

		// We always use at least a little transition time so that the code is more trivial
		fadeTime = Mathf.Max(fadeTime, 0.0001f); 

		this.fadeTimeTotal = fadeTime;
		this.fadeTimeCurrent = 0f;
	}

	private class MusicHelper {
		public AudioSource source;
		public float initialVolume;
		public float targetVolume;
	}
}
