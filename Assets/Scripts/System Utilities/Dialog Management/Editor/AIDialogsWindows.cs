using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utilities.UI;
using Utilities.VariableManagement;

namespace Utilities.Dialog
{
    public class AIDialogsWindows : EditorWindow
    {
        private static AIDialogsWindows _window;
        private static AIDialogScriptableObject _dialogsLibraryAsset;
        private static List<DialogConversationUnit> _listDialogConversations;
        
        private static DialogsPopulator _dialogLibraryPopulator;

        private static Vector2 _scrollPosition;
        private static bool[] _collapseState;

        [MenuItem("TFW Tools/Load AI Dialogs")]
        public static void Initialize()
        {
            if (_window == null)
                _window = GetWindow();

            _dialogLibraryPopulator = new DialogsPopulator();
            _dialogLibraryPopulator.InitializeDialogLibrary();

            LoadDialogsLibrary();
        }

        public static AIDialogsWindows GetWindow()
        {
            return (AIDialogsWindows)GetWindow(typeof(AIDialogsWindows), false, "AI Dialogs");
        }

        public void OnGUI()
        {
            if (_listDialogConversations == null || _listDialogConversations.Count != Enum.GetValues(typeof(DialogEnum)).Length)
                LoadDialogsLibrary();

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            {
                if (_listDialogConversations != null && _listDialogConversations.Count > 0)
                {
                    DrawDialogsLibraryList();
                }
            }
            GUILayout.EndScrollView();
        }

        private static void LoadDialogsLibrary()
        {
            _dialogsLibraryAsset = Resources.Load<AIDialogScriptableObject>("AIDialogs");

            _listDialogConversations = _dialogsLibraryAsset.GameDialogs;
            _listDialogConversations.Sort((a, b) => a.dialogName.CompareTo(b.dialogName));

            _collapseState = new bool[_listDialogConversations.Count];
        }

        private void DrawDialogsLibraryList()
        {
            for (int i = 0; i < _listDialogConversations.Count; i++)
            {
                _collapseState[i] = EditorGUILayout.Foldout(_collapseState[i], _listDialogConversations[i].dialogName.ToString());

                if (_collapseState[i])
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        Editor.CreateEditor(_listDialogConversations[i].conversation).OnInspectorGUI();
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