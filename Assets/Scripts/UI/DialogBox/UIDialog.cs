using System;
using System.Collections;
using System.Collections.Generic;
using GameManagers;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Utilities;
using Utilities.Audio;
using Utilities.Dialog;
using Utilities.UI;
using Utilities.VariableManagement;

namespace UI.Dialog
{
    public class UIDialog : MonoBehaviour, IUIDialogText, IUpdateBehaviour
    {
        [SerializeField] private Animator _hudAnimator;
        [SerializeField] private TextMeshProUGUI _speakerText;
        [SerializeField] private TextMeshProUGUI _dialogText;
        [SerializeField] private UIAnimationEventHandler _dialogEventHandler;

        private const string SHOW_DIALOG_HUD_ANIMATION = "Show";
        private const string HIDE_DIALOG_HUD_ANIMATION = "Hide";

        private const string ENABLE_SKIP_TEXT_ANIMATION = "Enable";
        private const string DISABLE_SKIP_TEXT_ANIMATION = "Disable";

        private static DialogsPopulator _dialogLibraryPopulator;
        private static AIDialogScriptableObject _dialogsLibraryAsset;

        private Queue<DialogTextUnit> _conversationQueue = new Queue<DialogTextUnit>();
        private string _currentDialogText = "";
        private bool _visible = false;
        private bool _dialogEnded = false;
        private bool _enableInputAfterDialog = true;
        private Action _dialogEndCallback;

        public void RunUpdate()
        {
            if (GameStateManager.currentState != GameState.RUNNING) return;
            if (InputController.UI.SkipDialog() && !_dialogEnded)
                DisplayNextDialog();
        }

        public void StartDialog(DialogEnum p_dialogName, Action p_onDialogEnd = null, bool p_enableInputAfterDialog = true)
        {
            _dialogEndCallback = p_onDialogEnd;
            _enableInputAfterDialog = p_enableInputAfterDialog;
            _dialogEnded = false;

            InitializeDialog(p_dialogName);
            Show();
        }

        private void InitializeDialog(DialogEnum p_dialogName)
        {
            _conversationQueue.Clear();

            _speakerText.text = "";
            _dialogText.text = "";

            _dialogLibraryPopulator = new DialogsPopulator();
            _dialogLibraryPopulator.InitializeDialogLibrary();

            _dialogsLibraryAsset = Resources.Load<AIDialogScriptableObject>("AIDialogs");

            foreach (DialogConversationUnit __dialogConversation in _dialogsLibraryAsset.GameDialogs)
            {
                if (p_dialogName.ToString() == __dialogConversation.dialogName)
                {
                    foreach (DialogTextUnit __conversationUnit in __dialogConversation.conversation.conversationTexts)
                    {
                        _conversationQueue.Enqueue(__conversationUnit);
                    }
                    break;
                }
            }
        }

        private void Show()
        {
            InputController.GamePlay.InputEnabled = false;
            InputController.GamePlay.MouseEnabled = false;
            if (!_visible)
            {
                AudioManager.instance.Play(AudioNameEnum.UI_DIALOG_START);
                _hudAnimator.Play(SHOW_DIALOG_HUD_ANIMATION);
                _visible = true;
            }
            else
                DisplayNextDialog();
        }

        [UsedImplicitly]
        private void StartShowText()
        {
            DisplayNextDialog();

            InputController.UI.InputEnabled = true;
        }

        [UsedImplicitly]
        private void DisablingHud()
        {
            if (_enableInputAfterDialog)
            {
                InputController.GamePlay.InputEnabled = true;
                InputController.GamePlay.MouseEnabled = true;
            }

            InputController.UI.InputEnabled = false;
            _visible = false;
        }

        private void DisplayNextDialog()
        {
            StopAllCoroutines();
            AudioManager.instance.Stop(AudioNameEnum.UI_DIALOG_TYPING);

            _dialogText.text = _currentDialogText;

            if (_conversationQueue.Count == 0)
            {
                EndDialog();
                return;
            }

            DialogTextUnit __conversationUnit = _conversationQueue.Dequeue();
            AudioManager.instance.Play(AudioNameEnum.UI_DIALOG_NEXT);
            StartCoroutine(TypeDialog(__conversationUnit));
        }

        private IEnumerator TypeDialog(DialogTextUnit p_conversationUnit)
        {
            _speakerText.text = p_conversationUnit.speaker.ToString();
            _dialogText.text = "";

            _currentDialogText = p_conversationUnit.text;

            AudioManager.instance.Play(AudioNameEnum.UI_DIALOG_TYPING, true);
            foreach (char __letter in _currentDialogText.ToCharArray())
            {
                while (GameStateManager.currentState == GameState.PAUSED)
                    yield return null;

                _dialogText.text += __letter;
                yield return null;
            }
            AudioManager.instance.Stop(AudioNameEnum.UI_DIALOG_TYPING);
        }

        private void EndDialog()
        {
            InputController.GamePlay.InputEnabled = true;
            InputController.GamePlay.MouseEnabled = true;
            AudioManager.instance.Stop(AudioNameEnum.UI_DIALOG_TYPING, 0.1f);
            AudioManager.instance.Play(AudioNameEnum.UI_DIALOG_END);
            _hudAnimator.Play(HIDE_DIALOG_HUD_ANIMATION);
            _dialogEnded = true;

            // TODO: Set callback to be called on animation end (like AnimationEventHandlers)
            _dialogEndCallback?.Invoke();
        }
    }
}
