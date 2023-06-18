using UnityEngine;
using UnityEngine.UI;

namespace ExtraTools.Test.Scripts
{
	public class DemoAudioPlayer : MonoBehaviour
	{
		[SerializeField] private AudioClip[] clips;
		[SerializeField] private AudioClip[] music;
		[SerializeField] private Button playRandomSound;
		[SerializeField] private Button changeMusic;
		[SerializeField] private Toggle playPauseMusic;
		[SerializeField] private Slider masterSound;
		[SerializeField] private Slider musicSound;
		[SerializeField] private Slider sfxSound;

		[SerializeField] private string masterLabel = "MasterVolume";
		[SerializeField] private string musicLabel = "MusicVolume";
		[SerializeField] private string sfxLabel = "SFXVolume";

		private void Awake()
		{
			playRandomSound.onClick.AddListener(OnPlayRandom);
			playPauseMusic.onValueChanged.AddListener(OnPlayMusic);
			changeMusic.onClick.AddListener(OnChangeMusic);
			masterSound.onValueChanged.AddListener(OnSetMaster);
			musicSound.onValueChanged.AddListener(OnSetMusic);
			sfxSound.onValueChanged.AddListener(OnSetSFX);
		}

		private void OnSetMaster(float val)
		{
			AudioPlayer.SetVolume(masterLabel, val, true);
		}

		private void OnSetMusic(float val)
		{
			AudioPlayer.SetVolume(musicLabel, val, true);
		}

		private void OnSetSFX(float val)
		{
			AudioPlayer.SetVolume(sfxLabel, val, true);
		}

		private void OnChangeMusic()
		{
			AudioPlayer.PlayMusic(music[Random.Range(0, music.Length)], musicLabel);
		}

		private void OnPlayMusic(bool val)
		{
			if (val)
			{
				AudioPlayer.PlayMusic(music[0], musicLabel, 1);
				return;
			}

			AudioPlayer.StopMusic(musicLabel);
		}

		private void OnPlayRandom()
		{
			AudioPlayer.PlayOneShot(clips[Random.Range(0, clips.Length)]);
		}
	}
}