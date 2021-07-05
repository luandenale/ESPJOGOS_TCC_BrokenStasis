using System;
using GameManagers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using Utilities.Audio;

namespace UI.GameOver
{
    public class UIGameOver : MonoBehaviour
    {
        [SerializeField] private Button _buttonLoadLastCheckpoint;
        [SerializeField] private Button _buttonBackToTitleScreen;

        private Animator _animator;
        private const string ANIMATION_SHOW_PANEL = "Show";

        private void Start()
        {
            _animator = GetComponent<Animator>() ?? throw new MissingComponentException("Animator not found!");

            if (!_buttonLoadLastCheckpoint) throw new MissingFieldException("Button LoadLastCheckpoint not assigned.");
            if (!_buttonBackToTitleScreen) throw new MissingFieldException("Button No not assigned.");
        }

        public void StartUIHandlers(string p_textTitle = null,
                                    Action p_handleLoadLastCheckPointOnClick = null,
                                    Action p_handleBackToTitleScreenOnClick = null)
        {
            SetButtonsInteractable(false);

            _buttonLoadLastCheckpoint?.onClick.RemoveAllListeners();
            _buttonBackToTitleScreen?.onClick.RemoveAllListeners();

            _buttonLoadLastCheckpoint?.onClick.AddListener(delegate
            {
                HandleLoadLastCheckpointOnClick(p_handleLoadLastCheckPointOnClick);
            });

            _buttonBackToTitleScreen?.onClick.AddListener(delegate
            {
                HandleBackToTitleScreenOnClick(p_handleBackToTitleScreenOnClick);
            });

            AudioManager.instance.PlayMusic(AudioNameEnum.SOUND_TRACK_GAMEOVER, 5);
            Show();
        }

        private void SetButtonsInteractable(bool p_interactable)
        {
            _buttonLoadLastCheckpoint.interactable = p_interactable;
            _buttonBackToTitleScreen.interactable = p_interactable;
        }

        private void HandleLoadLastCheckpointOnClick(Action p_handleLoadLastCheckPointOnClick = null)
        {
            SetButtonsInteractable(false);

            GameHudManager.instance.optionsLoadingUI.FadeIn(delegate
            {
                SceneManager.LoadScene(ScenesConstants.GAME);
                GameStateManager.SetGameState(GameState.RUNNING);
                Time.timeScale = 1;
                p_handleLoadLastCheckPointOnClick?.Invoke();
            });
        }

        private void HandleBackToTitleScreenOnClick(Action p_handleBackToTitleScreenOnClick = null)
        {
            SetButtonsInteractable(false);

            GameHudManager.instance.optionsLoadingUI.FadeIn(delegate
            {
                SceneManager.LoadScene(ScenesConstants.MENU);
                GameStateManager.SetGameState(GameState.RUNNING);
                Time.timeScale = 1;
                p_handleBackToTitleScreenOnClick?.Invoke();
            });
        }

        public void Show()
        {
            _animator.Play(ANIMATION_SHOW_PANEL);
        }

        private void HandleEndOfShowPanelAnimation()
        {
            SetButtonsInteractable(true);

            AudioManager.instance.FadeOutAllSounds(0.2f);
            Time.timeScale = 0;
        }
    }
}
