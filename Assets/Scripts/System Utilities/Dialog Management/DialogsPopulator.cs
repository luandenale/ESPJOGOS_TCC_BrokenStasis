using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Utilities.UI;
using Utilities.VariableManagement;

namespace Utilities.Dialog
{
    public class DialogsPopulator
    {
        private static AIDialogScriptableObject _dialogLibrary;

        private const string DIALOG_CONVERSATION_RESOURCES_PATH = "DialogConversations/";
        private const string DIALOG_LIB_FULL_PATH = "Assets/Scripts/System Utilities/Dialog Management/Resources/AIDialogs.asset";
        private const string DIALOG_CONVERSATION_FULL_PATH = "Assets/Scripts/System Utilities/Dialog Management/Resources/DialogConversations/";

        public void InitializeDialogLibrary()
        {
            PopulateDialogAssets();
            LoadLibrary();
            RemoveMissingDialogs();
            BuildLibrary();
        }

        private static void LoadLibrary()
        {
            _dialogLibrary = Resources.Load<AIDialogScriptableObject>("AIDialogs");

            if (_dialogLibrary == null)
            {
                _dialogLibrary = new AIDialogScriptableObject();
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(_dialogLibrary, DIALOG_LIB_FULL_PATH);
#endif
            }
        }

        private void PopulateDialogAssets()
        {
            foreach (var __dialogNameEnum in Enum.GetValues(typeof(DialogEnum)))
            {
                if (!DialogExists(__dialogNameEnum.ToString()))
                    CreateDialogUnit(__dialogNameEnum.ToString());
            }
        }

        private void BuildLibrary()
        {
            foreach (var __dialogNameEnum in Enum.GetValues(typeof(DialogEnum)))
            {
                DialogConversationUnit __dialogConversation = new DialogConversationUnit()
                {
                    dialogName = __dialogNameEnum.ToString(),
                    conversation = Resources.Load<DialogTextConversation>(DIALOG_CONVERSATION_RESOURCES_PATH + __dialogNameEnum.ToString())
                };             

                AddDialogToLibrary(__dialogNameEnum, __dialogConversation);
            }
        }

        private static void AddDialogToLibrary(object p_dialogNameEnum, DialogConversationUnit p_dialogConversation)
        {
            bool __shouldAddNewEntry = true;
            
            foreach (var __dialogConversation in _dialogLibrary.GameDialogs)
            {
                if (__dialogConversation.dialogName == p_dialogNameEnum.ToString())
                    __shouldAddNewEntry = false;
            }

            if (__shouldAddNewEntry)
            {
                _dialogLibrary.GameDialogs.Add(p_dialogConversation);
            }
            else
            {
                _dialogLibrary.GameDialogs[_dialogLibrary.GameDialogs.FindIndex(__index => __index.dialogName.Equals(p_dialogNameEnum.ToString()))] = p_dialogConversation;
            }
        }

        private void RemoveMissingDialogs()
        {
            foreach (var __dialogConversation in _dialogLibrary.GameDialogs)
            {
                if (!Enum.IsDefined(typeof(DialogEnum), __dialogConversation.dialogName.ToString()))
                {
#if UNITY_EDITOR
                    AssetDatabase.DeleteAsset(DIALOG_CONVERSATION_FULL_PATH + __dialogConversation.dialogName + ".asset");
#endif
                    _dialogLibrary.GameDialogs.Remove(__dialogConversation);
                }
            }
        }

        private bool DialogExists(string p_dialogName)
        {
            return Resources.Load<DialogTextConversation>(DIALOG_CONVERSATION_RESOURCES_PATH + p_dialogName) != null;
        }

        private void CreateDialogUnit(string p_audioName)
        {
            DialogTextConversation __newDialogConversation = new DialogTextConversation();
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(__newDialogConversation, DIALOG_CONVERSATION_FULL_PATH + p_audioName + ".asset");
#endif
        }
    }
}