using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace ExtraTools
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private int initializeSources = 4;
        [SerializeField, Range(-80, 20)] private float minimumVolume = -80;
        [SerializeField, Range(-80, 20)] private float maximumVolume = 0;
        [SerializeField] private float volumeChangeSpeed = 7;

        private static AudioMixer _mixer;
        private static AudioMixerGroup _sfxMixer;
        private static List<AudioSource> _sources = new List<AudioSource>();
        private static AudioSource _musicSource;
        private static GameObject _thisObject;
        private static Dictionary<string, IEnumerator> _activeCoroutines = new Dictionary<string, IEnumerator>();
        private static float _minVolume = -80;
        private static float _maxVolume = 20;
        private static float _volumeSpeed = 7;

        #region Unity Methods

        private void OnDisable()
        {
            foreach (IEnumerator coroutine in _activeCoroutines.Values)
            {
                StopCoroutine(coroutine);
            }

            if (_musicSource)
                Destroy(_musicSource);

            AudioSource[] sources = _thisObject.GetComponents<AudioSource>();

            for (int i = 0; i < sources.Length; i++)
            {
                Destroy(sources[i]);
            }

            _musicSource = null;
            _activeCoroutines.Clear();
            _sources.Clear();
        }

        private void Awake()
        {
            Set(Resources.Load<AudioMixer>("ExtraTools/Mixer"), minimumVolume, maximumVolume, volumeChangeSpeed);
        }

        #endregion

        /// <summary>
        /// Initialized the AudioPlayer. Use it to override default values
        /// </summary>
        /// <param name="mixer">New mixer to use</param>
        /// <param name="minVolume">Minimum volume to be set in the mixer</param>
        /// <param name="maxVolume">Maximum volume to be set in the mixer</param>
        /// <param name="volumeSpeed">How fast the volume should increase/decrease when music stops</param>
        /// <param name="musicMixerGroup">Mixer group name which controls the music volume</param>
        /// <param name="sfxMixerGroup">Mixer group name which controls the SFX</param>
        private void Set(AudioMixer mixer = null, float minVolume = -80, float maxVolume = 20, float volumeSpeed = 5,
            string musicMixerGroup = "Music", string sfxMixerGroup = "SFX")
        {
            _mixer = mixer;
            _minVolume = minVolume;
            _maxVolume = maxVolume;
            _volumeSpeed = volumeSpeed;
            _thisObject = gameObject;

            AudioMixerGroup[] sfxGroups = _mixer.FindMatchingGroups(sfxMixerGroup);

            if (sfxGroups.Length > 0)
            {
                _sfxMixer = sfxGroups[0];

                for (int i = 0; i < initializeSources; i++)
                {
                    AudioSource source = _thisObject.AddComponent<AudioSource>();
                    source.outputAudioMixerGroup = sfxGroups[0];
                    _sources.Add(source);
                }
            }
            else
            {
                Debug.LogWarning(
                    $"Mixer '{_mixer}' does not have an sfx group '{sfxMixerGroup}'. Volume settings won't work!",
                    _mixer);
            }

            _musicSource = _thisObject.AddComponent<AudioSource>();

            AudioMixerGroup[] musicGroups = _mixer.FindMatchingGroups(musicMixerGroup);

            if (musicGroups.Length > 0)
            {
                _musicSource.outputAudioMixerGroup = musicGroups[0];
            }
            else
            {
                Debug.LogWarning(
                    $"Mixer '{_mixer}' does not have a music group '{musicMixerGroup}'. Volume settings won't work!",
                    _mixer);
            }
        }

        /// <summary>
        /// Plays a clip using an individual audio source. Creates one if all sources are currently busy
        /// </summary>
        /// <param name="clip">Clip to play</param>
        /// <param name="minPitch">Minimum pitch</param>
        /// <param name="maxPitch">Maximum pitch</param>
        public static void PlayOneShot(AudioClip clip, float minPitch = 1, float maxPitch = 1)
        {
            for (int i = 0; i < _sources.Count; i++)
            {
                if (_sources[i].isPlaying) continue;

                _sources[i].pitch = Random.Range(minPitch, maxPitch);
                _sources[i].PlayOneShot(clip);
                return;
            }

            AudioSource source = _thisObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = _sfxMixer;
            source.loop = false;
            source.pitch = Random.Range(minPitch, maxPitch);
            source.PlayOneShot(clip);
            _sources.Add(source);
        }

        /// <summary>
        /// Plays a music in a dedicated audio source. If volume label is set fades out/in the track.
        /// </summary>
        /// <param name="clip">Track to play</param>
        /// <param name="volumeLabel">Exposed volume parameter label in the mixer</param>
        /// <param name="volume">Volume to play the next track at</param>
        /// <param name="isLooping">Should the track loop?</param>
        public static void PlayMusic(AudioClip clip, string volumeLabel = "", float volume = -1, bool isLooping = true)
        {
            if (!volumeLabel.IsValid())
            {
                _musicSource.clip = clip;
                _musicSource.loop = isLooping;
                _musicSource.Play();
                return;
            }

            if (volume < 0)
                _mixer.GetFloat(volumeLabel, out volume);

            SetVolume(volumeLabel, 0, callback: () => {
                _musicSource.clip = clip;
                _musicSource.loop = isLooping;
                _musicSource.Play();
                SetVolume(volumeLabel, (volume - _minVolume) / (_maxVolume - _minVolume));
            });
        }

        /// <summary>
        /// Stops the current music. If the label is set will fade out before stopping
        /// </summary>
        /// <param name="volumeLabel">Exposed volume parameter label in the mixer</param>
        public static void StopMusic(string volumeLabel = "")
        {
            if (!volumeLabel.IsValid() || !_mixer.GetFloat(volumeLabel, out float currentVolume))
            {
                _musicSource.Stop();
                _musicSource.clip = null;
                return;
            }

            SetVolume(volumeLabel, 0, callback: () => {
                _musicSource.Stop();
                _musicSource.clip = null;
                SetVolume(volumeLabel, currentVolume, true);
            });
        }

        /// <summary>
        /// Sets the volume for a given label.
        /// </summary>
        /// <param name="volumeLabel">Label of an exposed variable on the mixer</param>
        /// <param name="volume">Desired volume level between 0-1</param>
        /// <param name="instant">Should the volume be changed instantly or gradually</param>
        /// <param name="callback">Invoked after the volume is set to desired level</param>
        public static void SetVolume(string volumeLabel, float volume, bool instant = false, Action callback = null)
        {
            float range = _maxVolume - _minVolume;

            if (instant)
            {
                _mixer.SetFloat(volumeLabel, _minVolume + range * volume);
                callback?.Invoke();
                return;
            }

            if (_activeCoroutines.TryGetValue(volumeLabel, out IEnumerator coroutine))
                CoroutineStarter.Get.StopCoroutine(coroutine);
            else
                _activeCoroutines.Add(volumeLabel, null);

            _activeCoroutines[volumeLabel] = VolumeCoroutine(volumeLabel, _minVolume + range * volume, callback);
            CoroutineStarter.Get.StartCoroutine(_activeCoroutines[volumeLabel]);
        }

        /// <summary>
        /// Sets the volume gradually
        /// </summary>
        /// <param name="volumeLabel">Label of an exposed variable on the mixer</param>
        /// <param name="setTo">Desired volume level between minimumVolume-maximumVolume</param>
        /// <param name="callback">Invoked after the volume is set to desired level</param>
        private static IEnumerator VolumeCoroutine(string volumeLabel, float setTo, Action callback = null)
        {
            _mixer.GetFloat(volumeLabel, out float currentVolume);

            while (Mathf.Abs(currentVolume - setTo) > 0.025f)
            {
                currentVolume = Mathf.Lerp(currentVolume, setTo, Time.deltaTime * _volumeSpeed);
                _mixer.SetFloat(volumeLabel, currentVolume);
                yield return null;
            }

            _mixer.SetFloat(volumeLabel, setTo);
            _activeCoroutines.Remove(volumeLabel);
            callback?.Invoke();
        }
    }
}