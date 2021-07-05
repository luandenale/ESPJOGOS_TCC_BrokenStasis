using System;
using GameManagers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI.Minigame
{
    public class MinigameUI : MonoBehaviour
    {
        [SerializeField] private Animator _hudAnimator;
        [SerializeField] private Image[] _codeImages;
        [SerializeField] private Button[] _buttons;
        [SerializeField] private TextMeshProUGUI _countdownText;
        [SerializeField] private Sprite _collectedImage;
        [SerializeField] private UIAnimationEventHandler _minigameEventHandler;

        private MinigameLogic _minigameLogic;
        private MinigameStateEnum _minigameEndState = MinigameStateEnum.NONE;

        public Action onMinigameSuccess;
        public Action onMinigameFailed;

        private const string SHOW_MINIGAME_HUD_ANIMATION = "Show";
        private const string HIDE_MINIGAME_HUD_ANIMATION = "Hide";
        private const string PAUSE_GAME_HUD_ANIMATION = "Pause";
        private const string RESUME_GAME_HUD_ANIMATION = "Resume";

        private void Awake()
        {
            _minigameEventHandler.OnHideAnimationEnd = HandleHideAnimationEnd;
            _minigameEventHandler.OnShowAnimationEnd = HandleShowAnimationEnd;

            _minigameLogic = new MinigameLogic(
                _codeImages,
                _buttons,
                _countdownText,
                _collectedImage
            );

            _minigameLogic.onMinigameFinished = HandleMinigameEnded;
            GameStateManager.onStateChanged += HandleGameStateChanged;
        }

        private void HandleGameStateChanged(GameState p_gameState)
        {
            switch (p_gameState)
            {
                case GameState.PAUSED:
                    HandlePauseGame();
                    break;
                case GameState.RUNNING:
                    HandleResumeGame();
                    break;
                default:
                    break;
            }
        }

        private void HandleMinigameEnded(MinigameStateEnum p_finishedState)
        {
            _hudAnimator.Play(HIDE_MINIGAME_HUD_ANIMATION);
            _minigameEndState = p_finishedState;
        }

        private void HandlePauseGame()
        {
            if (_minigameEndState == MinigameStateEnum.PLAYING)
            {
                foreach (Image __image in _codeImages)
                    SetImageAlpha(0, __image);
                foreach(Button __button in _buttons)
                    SetButtonCondition(__button, false);
            }
        }

        private void HandleResumeGame()
        {
            if (_minigameEndState == MinigameStateEnum.PLAYING)
            {
                foreach (Image __image in _codeImages)
                    SetImageAlpha(1, __image);
                foreach(Button __button in _buttons)
                    SetButtonCondition(__button, true);
            }
        }

        private void SetButtonCondition(Button p_button, bool p_activate)
        {
            p_button.interactable = p_activate;
            SetImageAlpha(Convert.ToInt32(p_activate), p_button.GetComponent<Image>());
        }

        private void SetImageAlpha(int p_alpha, Image p_image)
        {
            Color __color = p_image.color;
            __color.a = p_alpha;
            p_image.color = __color;
        }

        private void HandleHideAnimationEnd()
        {
            InputController.GamePlay.InputEnabled = true;

            switch (_minigameEndState)
            {
                case MinigameStateEnum.SUCCESSFULL:
                    onMinigameSuccess?.Invoke();
                    break;
                case MinigameStateEnum.FAILED:
                    onMinigameFailed?.Invoke();
                    break;
                default:
                    break;
            }
        }

        private void HandleShowAnimationEnd()
        {
            _minigameLogic.StartCountDown();
        }

        public void ShowMinigame()
        {
            InputController.GamePlay.InputEnabled = false;
            InputController.GamePlay.MouseEnabled = false;

            _minigameEndState = MinigameStateEnum.PLAYING;

            _minigameLogic.InitializeMinigame();

            _hudAnimator.Play(SHOW_MINIGAME_HUD_ANIMATION);
        }
    }
}