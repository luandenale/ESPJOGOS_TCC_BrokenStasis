using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utilities.VariableManagement;

namespace Utilities.Audio
{
    public class AudioLibraryWindow : EditorWindow
    {
        private static AudioLibraryWindow _window;
        private static AudioLibraryScriptableObject _audioLibraryAsset;
        private static List<AudioClipUnit> _listAudioClips;
        private static AudioLibraryPopulator _audioLibraryPopulator;

        private static Vector2 _scrollPosition;
        private static bool[] _collapseState;

        [MenuItem("TFW Tools/Load Audio Library")]
        public static void Initialize()
        {
            if (_window == null)
                _window = GetWindow();

            _audioLibraryPopulator = new AudioLibraryPopulator();
            _audioLibraryPopulator.InitializeAudioLibrary();

            LoadAudioLibrary();
        }

        public static AudioLibraryWindow GetWindow()
        {
            return (AudioLibraryWindow)GetWindow(typeof(AudioLibraryWindow), false, "Audio Library");
        }

        public void OnGUI()
        {
            if (_listAudioClips == null || _listAudioClips.Count != Enum.GetValues(typeof(AudioNameEnum)).Length)
                LoadAudioLibrary();

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            {
                if (_listAudioClips != null && _listAudioClips.Count > 0)
                {
                    DrawAudioLibraryList();
                }
            }
            GUILayout.EndScrollView();
        }

        private static void LoadAudioLibrary()
        {
            _audioLibraryAsset = Resources.Load<AudioLibraryScriptableObject>("AudioLibrary");

            _listAudioClips = _audioLibraryAsset.AudioLibrary;
            _listAudioClips.Sort((a, b) => a.audioName.CompareTo(b.audioName));

            _collapseState = new bool[_listAudioClips.Count];
        }

        private void DrawAudioLibraryList()
        {
            for (int i = 0; i < _listAudioClips.Count; i++)
            {
                _collapseState[i] = EditorGUILayout.Foldout(_collapseState[i], _listAudioClips[i].audioName.ToString());

                if (_collapseState[i])
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        Editor.CreateEditor(_listAudioClips[i].audioClipParams).OnInspectorGUI();
                    }
                    GUILayout.EndVertical();
                }
            }
        }

        public void OnInspectorUpdate()
        {
            this.Repaint();
        }
    }
}