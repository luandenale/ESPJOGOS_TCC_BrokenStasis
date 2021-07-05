using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace Utilities.VariableManagement
{
    public class VariablesWindow : EditorWindow
    {
        private const string VARIABLES_PATH = "Assets/Scripts/System Utilities/Variables Management/Resources/Variables";
        private static VariablesWindow _window;
        private static List<ScriptableObject> _listVariablesAssets = new List<ScriptableObject>();

        private static Vector2 _scrollPosition;
        private static bool[] _collapseState;

        private static FileInfo[] _fileInfoArray;

        [MenuItem("TFW Tools/Variables")]
        public static void Initialize()
        {
            if (_window == null)
                _window = GetWindow();

            LoadVariables();
        }

        public static VariablesWindow GetWindow()
        {
            return (VariablesWindow)GetWindow(typeof(VariablesWindow), false, "Variables");
        }

        public void OnGUI()
        {
            if (_listVariablesAssets == null || _listVariablesAssets.Count != (_fileInfoArray.Length/2))
                LoadVariables();

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            {
                if (_listVariablesAssets != null && _listVariablesAssets.Count > 0)
                {
                    DrawVariablesList();
                }
            }
            GUILayout.EndScrollView();
        }

        private static void LoadVariables()
        {
            DirectoryInfo __info = new DirectoryInfo(VARIABLES_PATH);
            _fileInfoArray = __info.GetFiles();
            
            _listVariablesAssets = new List<ScriptableObject>();

            foreach(FileInfo __file in _fileInfoArray)
            {
                if(!__file.Name.Contains("meta"))
                {
                    string __fileName = __file.Name.Split('.')[0];
                    _listVariablesAssets.Add(Resources.Load<ScriptableObject>("Variables/" + __fileName));
                }
            }

            _collapseState = new bool[_listVariablesAssets.Count];
        }
    
        private void DrawVariablesList()
        {
            for (int i = 0; i < _listVariablesAssets.Count; i++)
            {
                _collapseState[i] = EditorGUILayout.Foldout(_collapseState[i], _listVariablesAssets[i].name);
                
                if (_collapseState[i])
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        Editor.CreateEditor(_listVariablesAssets[i]).OnInspectorGUI();
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
