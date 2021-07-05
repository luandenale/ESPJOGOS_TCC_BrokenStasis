using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Utilities.Audio;
using Utilities.VariableManagement;

namespace GameManagers
{
    public enum AudioRange
    {
        LOW = 8,
        MEDIUM = 15,
        HIGH = 20
    }

    public class AudioManager : MonoBehaviour
    {
        private static AudioLibraryScriptableObject _audioLibrary;

        private static AudioManager _instance;
        private static GameObject _audioManagerGameObject;
        private static GameObject _audioSourcePoolGameObject;
        private static AudioSourcePool _audioSourcePool;
        private static AudioLibraryPopulator _audioLibraryPopulator;
        private List<AudioSource> _pausedAudioSources = new List<AudioSource>();
        private AudioNameEnum _currentMusicName;
        private AudioSource _currentMusicAudioSource;

        public static AudioManager instance
        {
            get
            {
                if (_instance == null)
                {
                    InitializeAudioManager();
                }
                return _instance;
            }
        }

        private static void InitializeAudioManager()
        {
            _audioLibraryPopulator = new AudioLibraryPopulator();
            _audioLibraryPopulator.InitializeAudioLibrary();

            _audioLibrary = Resources.Load<AudioLibraryScriptableObject>("AudioLibrary");

            _audioManagerGameObject = new GameObject("AudioManager");
            _audioManagerGameObject.AddComponent<AudioManager>();

            if (_audioSourcePool == null)
                _audioSourcePoolGameObject = new GameObject("AudioSourcePool");

            _audioSourcePool = new AudioSourcePool(_audioSourcePoolGameObject.transform);
            _audioSourcePoolGameObject.transform.SetParent(_audioManagerGameObject.transform);

            _instance = _audioManagerGameObject.GetComponent<AudioManager>();
            DontDestroyOnLoad(_audioManagerGameObject);
        }

        public AudioSource Play(AudioNameEnum p_audio, bool p_loop = false, Action p_onAudioEnd = null, bool p_canRepeat = true)
        {
            AudioSource __audioSource = _audioSourcePool.GetFreeAudioSource(_pausedAudioSources);
            AudioClipParams __audioClipParams = _audioLibrary.AudioLibrary.Find(clip => clip.audioName.Equals(p_audio.ToString())).audioClipParams;

            if (!p_canRepeat && _audioSourcePool.IsAlreadyPlayingClip(__audioClipParams.audioFile))
            {
                // Debug.Log("Audio source already playing and should not repeat: " + p_audio);
                return null;
            }

            if (!__audioClipParams)
            {
                Debug.LogError("Audio manager: audioclip not found: " + p_audio.ToString());
                return null;
            }

            // Debug.Log("Started sound " + p_audio);

            __audioSource.loop = p_loop;
            __audioSource.clip = __audioClipParams.audioFile;
            __audioSource.volume = __audioClipParams.volume;
            __audioSource.outputAudioMixerGroup = __audioClipParams.audioMixerGroup;

            __audioSource.spatialBlend = 0f;

            __audioSource.mute = false;
            __audioSource.Play();

            TFWToolKit.Timer(__audioSource.clip.length, delegate ()
            {
                if (!p_loop)
                    __audioSource.mute = true;
                p_onAudioEnd?.Invoke();
            });

            return __audioSource;
        }

        public AudioSource PlayAtPosition(AudioNameEnum p_audio, Vector3 p_position, bool p_loop = false, AudioRange p_audioRange = AudioRange.HIGH, bool p_canRepeat = true, bool p_createWave = false, string p_ownerName = null)
        {
            var __audioSource = Play(p_audio, p_loop, null, p_canRepeat);
            if (__audioSource == null) return null;

            __audioSource.gameObject.transform.position = p_position;
            __audioSource.spatialBlend = 1f;
            __audioSource.rolloffMode = AudioRolloffMode.Custom;
            __audioSource.maxDistance = (int)p_audioRange;

            __audioSource.mute = false;
            __audioSource.Play();

            if (p_createWave)
                GameManagers.VFXManager.instance.CreateNewSoundWave(p_ownerName, p_position, p_audioRange, p_loop);

            TFWToolKit.Timer(__audioSource.clip.length, delegate ()
            {
                if (!p_loop)
                    __audioSource.mute = true;
            });

            return __audioSource;
        }

        public void Stop(AudioNameEnum p_audio, float p_secondsToFadeOut = 0)
        {
            AudioClip __clip = _audioLibrary.AudioLibrary.Find(clip => clip.audioName.Equals(p_audio.ToString())).audioClipParams.audioFile;

            if (__clip != null)
            {
                foreach (AudioSource __audioSource in _audioSourcePool.GetAudiosWithClip(__clip))
                {
                    if (p_secondsToFadeOut == 0)
                        __audioSource?.Stop();
                    else
                        StartCoroutine(FadeOutSound(__audioSource, p_secondsToFadeOut));
                }
            }
        }

        public void PlayMusic(AudioNameEnum p_newMusic, float p_secondsTransitionFade = 0)
        {
            AudioSource __audioSource = _currentMusicAudioSource;
            AudioClipParams __audioClipParams = _audioLibrary.AudioLibrary.Find(clip => clip.audioName.Equals(p_newMusic.ToString())).audioClipParams;

            if (__audioSource == null && p_secondsTransitionFade == 0)
            {
                __audioSource = _audioSourcePool.GetFreeAudioSource(_pausedAudioSources);
                __audioSource.loop = true;
                __audioSource.clip = __audioClipParams.audioFile;
                __audioSource.volume = __audioClipParams.volume;
                __audioSource.spatialBlend = 0f;
                __audioSource.mute = false;
                __audioSource.outputAudioMixerGroup = __audioClipParams.audioMixerGroup;
                __audioSource.Play();
            }
            else if (__audioSource == null)
            {
                __audioSource = FadeIn(p_newMusic, p_secondsTransitionFade, null, true);
            }
            else if (p_secondsTransitionFade == 0)
            {
                __audioSource.Stop();
                __audioSource.clip = __audioClipParams.audioFile;
                __audioSource.volume = __audioClipParams.volume;
                __audioSource.spatialBlend = 0f;
                __audioSource.mute = false;
                __audioSource.outputAudioMixerGroup = __audioClipParams.audioMixerGroup;
                __audioSource.Play();
            }
            else
            {
                FadeOut(_currentMusicName, p_secondsTransitionFade);
                __audioSource = FadeIn(p_newMusic, p_secondsTransitionFade, null, true);
            }
            _currentMusicName = p_newMusic;
            _currentMusicAudioSource = __audioSource;
        }

        public void StopMusic(float p_secondsToFadeOut = 0)
        {
            if (_currentMusicAudioSource == null) return;

            if (p_secondsToFadeOut != 0)
            {
                StartCoroutine(FadeOutSound(_currentMusicAudioSource, p_secondsToFadeOut, delegate
                {
                    _currentMusicAudioSource = null;
                }));
            }
            else
            {
                _currentMusicAudioSource.Stop();
                _currentMusicAudioSource = null;
            }
        }

        public void FadeOut(AudioNameEnum p_audio, float p_secondsToFadeOut, Action p_handleAudioFadedOut = null)
        {
            AudioClip __clip = _audioLibrary.AudioLibrary.Find(clip => clip.audioName.Equals(p_audio.ToString())).audioClipParams.audioFile;

            if (__clip != null)
            {
                foreach (AudioSource __audioSource in _audioSourcePool.GetAudiosWithClip(__clip))
                {
                    StartCoroutine(FadeOutSound(__audioSource, p_secondsToFadeOut, p_handleAudioFadedOut));
                }
            }
        }

        public void FadeOutAllSounds(float p_secondsToFadeOut)
        {
            foreach (AudioSource __audioSource in _audioSourcePool.GetAllAudioSources())
            {
                if (__audioSource != _currentMusicAudioSource && __audioSource.isPlaying)
                    StartCoroutine(FadeOutSound(__audioSource, p_secondsToFadeOut));
            }
        }

        public AudioSource FadeInAtPosition(AudioNameEnum p_audio, float p_secondsToFadeIn, Vector3 p_position, AudioRange p_audioRange = AudioRange.HIGH, AudioSource p_audioSource = null, bool p_loopAudio = false, Action p_handleAudioFadedIn = null)
        {
            var __audioClipParams = _audioLibrary.AudioLibrary.Find(clip => clip.audioName.Equals(p_audio.ToString())).audioClipParams;
            var __audioSource = p_audioSource;

            if (__audioSource == null)
            {
                __audioSource = _audioSourcePool.GetFreeAudioSource(_pausedAudioSources);
            }

            __audioSource.clip = __audioClipParams.audioFile;
            __audioSource.gameObject.transform.position = p_position;
            __audioSource.volume = 0f;
            __audioSource.spatialBlend = 1f;
            __audioSource.outputAudioMixerGroup = __audioClipParams.audioMixerGroup;
            __audioSource.rolloffMode = AudioRolloffMode.Custom;
            __audioSource.maxDistance = (int)p_audioRange;

            if (__audioClipParams != null)
            {
                StartCoroutine(FadeInSound(__audioSource, p_secondsToFadeIn, __audioClipParams.volume, p_loopAudio, p_handleAudioFadedIn));
            }
            return __audioSource;
        }

        public AudioSource FadeIn(AudioNameEnum p_audio, float p_secondsToFadeIn, AudioSource p_audioSource = null, bool p_loopAudio = false, Action p_handleAudioFadedIn = null)
        {
            var __audioClipParams = _audioLibrary.AudioLibrary.Find(clip => clip.audioName.Equals(p_audio.ToString())).audioClipParams;
            var __audioSource = p_audioSource;

            if (__audioSource == null)
            {
                __audioSource = _audioSourcePool.GetFreeAudioSource(_pausedAudioSources);
            }

            __audioSource.clip = __audioClipParams.audioFile;
            __audioSource.volume = 0f;
            __audioSource.spatialBlend = 0f;
            __audioSource.mute = true;
            __audioSource.outputAudioMixerGroup = __audioClipParams.audioMixerGroup;

            if (__audioClipParams != null)
            {
                StartCoroutine(FadeInSound(__audioSource, p_secondsToFadeIn, __audioClipParams.volume, p_loopAudio, p_handleAudioFadedIn));
            }
            return __audioSource;
        }

        private IEnumerator FadeOutSound(AudioSource p_audioSource, float p_secondsToFadeOut, Action p_handleAudioFadedOut = null)
        {
            float currentTime = 0;
            float start = p_audioSource.volume;
            // Debug.Log("started fade out");

            while (currentTime < p_secondsToFadeOut)
            {
                currentTime += Time.deltaTime;
                p_audioSource.volume = Mathf.Lerp(start, 0, currentTime / p_secondsToFadeOut);
                yield return null;
            }

            p_audioSource.loop = false;
            p_audioSource.Stop();
            p_audioSource.mute = true;
            p_handleAudioFadedOut?.Invoke();
        }

        private IEnumerator FadeInSound(AudioSource p_audioSource, float p_secondsToFadeIn, float p_audioVolume, bool p_loopAudio = false, Action p_handleAudioFadedIn = null)
        {
            var __fractionedVolumeToIncreasePerSecond = p_audioVolume / p_secondsToFadeIn;

            p_audioSource.volume = 0.0f;
            p_audioSource.loop = p_loopAudio;
            p_audioSource.mute = false;
            p_audioSource.Play();

            float currentTime = 0;
            float start = p_audioSource.volume;

            while (currentTime < p_secondsToFadeIn)
            {
                currentTime += Time.deltaTime;
                p_audioSource.volume = Mathf.Lerp(start, p_audioVolume, currentTime / p_secondsToFadeIn);
                yield return null;
            }

            p_handleAudioFadedIn?.Invoke();
        }
        public void Pause(AudioNameEnum p_audio)
        {
            AudioClip __clip = _audioLibrary.AudioLibrary.Find(clip => clip.audioName.Equals(p_audio.ToString())).audioClipParams.audioFile;

            if (__clip != null)
            {
                foreach (AudioSource __audioSource in _audioSourcePool.GetAudiosWithClip(__clip))
                {
                    __audioSource?.Stop();

                    if (__audioSource != null)
                    {
                        if (__audioSource.isPlaying)
                            __audioSource.Pause();
                        else
                            __audioSource.Play();
                    }
                }
            }
        }
        public void PauseAllAudioSources()
        {
            _pausedAudioSources.Clear();
            foreach (AudioSource __audioSource in _audioSourcePool.GetAllAudioSources())
            {
                if (__audioSource.isPlaying)
                {
                    __audioSource.Pause();
                    _pausedAudioSources.Add(__audioSource);
                }
            }
        }

        public void ResumeAllAudioSources()
        {
            foreach (AudioSource __audioSource in _pausedAudioSources)
            {
                __audioSource.Play();
            }
        }
    }
}
