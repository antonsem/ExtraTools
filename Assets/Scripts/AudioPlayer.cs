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

        private bool _isSet = false;

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

        private void Set(AudioMixer mixer = null, float minVolume = -80, float maxVolume = 20, float volumeSpeed = 5,
            string musicMixerGroup = "Music", string sfxMixerGroup = "SFX", bool forceReset = false)
        {
            if (_isSet && !forceReset) return;
            _isSet = true;

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

        public static void PlayOneShot(AudioClip clip, float minPitch = 0, float maxPitch = 0)
        {
            for (int i = 0; i < _sources.Count; i++)
            {
                if (_sources[i].isPlaying) continue;

                _sources[i].pitch = Random.Range(1 - minPitch, 1 + maxPitch);
                _sources[i].PlayOneShot(clip);
                return;
            }

            AudioSource source = _thisObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = _sfxMixer;
            source.loop = false;
            source.pitch = Random.Range(1 - minPitch, 1 + maxPitch);
            source.PlayOneShot(clip);
            _sources.Add(source);
        }

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

        public static void SetVolume(string volumeLabel, float volume, bool instant = false, Action callback = null)
        {
            float range = _maxVolume - _minVolume;

            if (instant)
            {
                _mixer.SetFloat(volumeLabel, _minVolume + range * volume);
                return;
            }

            if (_activeCoroutines.TryGetValue(volumeLabel, out IEnumerator coroutine))
                CoroutineStarter.Get.StopCoroutine(coroutine);
            else
                _activeCoroutines.Add(volumeLabel, null);

            _activeCoroutines[volumeLabel] = VolumeCoroutine(volumeLabel, _minVolume + range * volume, callback);
            CoroutineStarter.Get.StartCoroutine(_activeCoroutines[volumeLabel]);
        }

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