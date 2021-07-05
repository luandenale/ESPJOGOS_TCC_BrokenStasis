using System;
using GameManagers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using Utilities.VariableManagement;

namespace UI.PauseMenu
{
    public class UIOptions : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProTitle;
        [SerializeField] private Button _buttonContinue;
        [SerializeField] private Button _buttonLoadLastCheckpoint;
        [SerializeField] private Button _buttonBackToTitleScreen;


        private Animator _animator;
        private Action _handleFadeAnimationInEvent;
        private const string ANIMATION_SHOW_PANEL = "Show";
        private const string ANIMATION_HIDE_PANEL = "Hide";
        private const string ANIMATION_FADE_BLACK = "Fade";

        private void Start()
        {
            _animator = GetComponent<Animator>() ?? throw new MissingComponentException("Animator not found!");

            if (!_textMeshProTitle) throw new MissingFieldException("TextMeshPro Title not assigned.");
            if (!_buttonContinue) throw new MissingFieldException("Button Resume not assigned.");
            if (!_buttonLoadLastCheckpoint) throw new MissingFieldException("Button LoadLastCheckpoint not assigned.");
            if (!_buttonBackToTitleScreen) throw new MissingFieldException("Button No not assigned.");
        }

        public void StartUIHandlers(string p_textTitle = null, Action p_handleResumeSelection = null, Action p_handleLoadLastCheckPointOnClick = null, Action p_handleBackToTitleScreenOnClick = null)
        {
            _textMeshProTitle.text = p_textTitle;

            _buttonContinue?.onClick.RemoveAllListeners();
            _buttonLoadLastCheckpoint?.onClick.RemoveAllListeners();
            _buttonBackToTitleScreen?.onClick.RemoveAllListeners();

            _buttonContinue?.onClick.AddListener(delegate
            {
                p_handleResumeSelection?.Invoke();
                Close();
            });

            _buttonLoadLastCheckpoint?.onClick.AddListener(delegate
           {
               HandleLoadLastCheckpointOnClick(p_handleLoadLastCheckPointOnClick);
               Close();
           });

            _buttonBackToTitleScreen?.onClick.AddListener(delegate
           {
               HandleBackToTitleScreenOnClick(p_handleBackToTitleScreenOnClick);
               Close();
           });

            _animator.Play(ANIMATION_SHOW_PANEL);
        }

        private void HandleLoadLastCheckpointOnClick(Action p_handleLoadLastCheckPointOnClick = null)
        {
            GameHudManager.instance.areyouSureUI.StartUIHandlers(delegate
            {
                GameStateManager.SetGameState(GameState.CUTSCENE);

                GameHudManager.instance.optionsLoadingUI.FadeIn(delegate
                {
                    SceneManager.LoadScene(ScenesConstants.GAME);
                    p_handleLoadLastCheckPointOnClick?.Invoke();
                });
            },
            delegate
            {
                Show();
            });
            Close();
        }

        private void HandleBackToTitleScreenOnClick(Action p_handleBackToTitleScreenOnClick = null)
        {
            GameHudManager.instance.areyouSureUI.StartUIHandlers(delegate
            {
                GameStateManager.SetGameState(GameState.CUTSCENE);

                GameHudManager.instance.optionsLoadingUI.FadeIn(delegate
                {
                    SceneManager.LoadScene(ScenesConstants.MENU);
                    p_handleBackToTitleScreenOnClick?.Invoke();
                });
            }, delegate
            {
                Show();
            });
            Close();
        }

        public void CloseAll()
        {
            if (GameHudManager.instance.areyouSureUI.isOpen)
                GameHudManager.instance.areyouSureUI.Close();
            Close();
        }

        private void Close()
        {
            _animator.Play(ANIMATION_HIDE_PANEL);
        }

        public void Show()
        {
            _animator.Play(ANIMATION_SHOW_PANEL);
        }
    }
}
