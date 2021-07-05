using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Utilities.VariableManagement;

namespace Utilities.Audio
{
    public class AudioLibraryPopulator
    {
        private static AudioLibraryScriptableObject _audioLibrary;

        private const string AUDIO_CLIP_PARAM_RESOURCES_PATH = "AudioClipParams/";
        private const string AUDIO_LIBRARY_FULL_PATH = "Assets/Scripts/System Utilities/Audio Management/Resources/AudioLibrary.asset";
        private const string AUDIO_CLIP_PARAM_FULL_PATH = "Assets/Scripts/System Utilities/Audio Management/Resources/AudioClipParams/";

        public void InitializeAudioLibrary()
        {
            PopulateAudioAssets();
            LoadLibrary();
            RemoveMissingAudios();
            BuildLibrary();
        }

        private static void LoadLibrary()
        {
            _audioLibrary = Resources.Load<AudioLibraryScriptableObject>("AudioLibrary");

            if (_audioLibrary == null)
            {
                _audioLibrary = new AudioLibraryScriptableObject();
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(_audioLibrary, AUDIO_LIBRARY_FULL_PATH);
#endif
            }
        }

        private void PopulateAudioAssets()
        {
            foreach (var __audioNameEnum in Enum.GetValues(typeof(AudioNameEnum)))
            {
                if (!AudioParamExists(__audioNameEnum.ToString()))
                    CreateAudioParam(__audioNameEnum.ToString());
            }
        }

        private void BuildLibrary()
        {
            foreach (var __audioNameEnum in Enum.GetValues(typeof(AudioNameEnum)))
            {
                AudioClipUnit __audioClipUnit = new AudioClipUnit()
                {
                    audioName = __audioNameEnum.ToString(),
                    audioClipParams = Resources.Load<AudioClipParams>(AUDIO_CLIP_PARAM_RESOURCES_PATH + __audioNameEnum.ToString())
                };

                AddAudioClipsToLibrary(__audioNameEnum, __audioClipUnit);
            }
        }

        private static void AddAudioClipsToLibrary(object p_audioNameEnum, AudioClipUnit p_audioClipUnit)
        {
            bool __shouldAddNewEntry = true;
            foreach (var __audioClipUnitInLib in _audioLibrary.AudioLibrary)
            {
                if (__audioClipUnitInLib.audioName == p_audioNameEnum.ToString())
                    __shouldAddNewEntry = false;
            }

            if (__shouldAddNewEntry)
            {
                _audioLibrary.AudioLibrary.Add(p_audioClipUnit);
            }
            else
            {
                _audioLibrary.AudioLibrary[_audioLibrary.AudioLibrary.FindIndex(__index => __index.audioName.Equals(p_audioNameEnum.ToString()))] = p_audioClipUnit;
            }
        }

        private void RemoveMissingAudios()
        {
            foreach (var __audioClipUnitInLib in _audioLibrary.AudioLibrary)
            {
                if (!Enum.IsDefined(typeof(AudioNameEnum), __audioClipUnitInLib.audioName.ToString()))
                {
#if UNITY_EDITOR
                    AssetDatabase.DeleteAsset(AUDIO_CLIP_PARAM_FULL_PATH + __audioClipUnitInLib.audioName + ".asset");
#endif
                    _audioLibrary.AudioLibrary.Remove(__audioClipUnitInLib);
                }
            }
        }

        private bool AudioParamExists(string p_audioName)
        {
            return Resources.Load<AudioClipParams>(AUDIO_CLIP_PARAM_RESOURCES_PATH + p_audioName) != null;
        }

        private void CreateAudioParam(string p_audioName)
        {
            AudioClipParams __newAudioClipUnit = new AudioClipParams();
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(__newAudioClipUnit, AUDIO_CLIP_PARAM_FULL_PATH + p_audioName + ".asset");
#endif
        }
    }
}