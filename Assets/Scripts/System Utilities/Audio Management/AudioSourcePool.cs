using System.Collections.Generic;
using GameManagers;
using UnityEngine;

namespace Utilities.Audio
{
    public class AudioSourcePool
    {
        private List<AudioSource> _sourcesPool = new List<AudioSource>();
        private Transform _poolParent;

        public AudioSourcePool(Transform p_poolParent)
        {
            _poolParent = p_poolParent;

            InstantiateNewAudioSource();
        }

        private AudioSource InstantiateNewAudioSource()
        {
            GameObject __newAudioSourceGameObject = new GameObject("AudioSource " + _sourcesPool.Count);
            __newAudioSourceGameObject.transform.SetParent(_poolParent);

            __newAudioSourceGameObject.AddComponent<AudioSource>();

            _sourcesPool.Add(__newAudioSourceGameObject.GetComponent<AudioSource>());

            return _sourcesPool[_sourcesPool.Count - 1];
        }

        public AudioSource GetFreeAudioSource(List<AudioSource> _pausedAudioSources)
        {
            foreach (AudioSource __audioSource in _sourcesPool)
            {
                if (!__audioSource.loop && !__audioSource.isPlaying && !_pausedAudioSources.Contains(__audioSource))
                {   
                    __audioSource.mute = true;
                    return __audioSource;
                }
            }
            
            return InstantiateNewAudioSource();
        }

        public List<AudioSource> GetAudiosWithClip(AudioClip p_audioClip)
        {
            List<AudioSource> __audioSources = new List<AudioSource>();

            foreach (AudioSource __audioSource in _sourcesPool)
            {
                if (__audioSource.clip == p_audioClip)
                    __audioSources.Add(__audioSource);
            }

            return __audioSources;
        }

        public List<AudioSource> GetAllAudioSources()
        {
            return _sourcesPool;
        }

        public bool IsAlreadyPlayingClip(AudioClip p_audioClip)
        {
            foreach (AudioSource __audioSource in _sourcesPool)
            {
                if (__audioSource.clip == p_audioClip && __audioSource.isPlaying)
                    return true;
            }
            return false;
        }
    }
}
