using UnityEngine;
using UnityEngine.Audio;

namespace Utilities.Audio
{
    public class AudioClipParams : ScriptableObject
    {
        public AudioClip audioFile;
        public AudioMixerGroup audioMixerGroup;
        [Range(0.0f, 1.0f)]
        public float volume = 1f;
    }
}