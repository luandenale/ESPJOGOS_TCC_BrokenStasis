using GameManagers;
using JetBrains.Annotations;
using SaveSystem;
using UnityEngine;
using Utilities;
using Utilities.Audio;
using Utilities.VariableManagement;

namespace UI.MainMenu
{
    public class MenuSceneController : MonoBehaviour
    {
        [SerializeField] private Animator _titleAnimator;
        [SerializeField] private TitleMenuAnimationEventHandler _titleAnimationHandler;
        [SerializeField] private MenuScreenAnimationController _mainMenuAnimationController;
        [SerializeField] private MenuScreenAnimationController _slotScreenAnimationController;
        [SerializeField] private MenuScreenAnimationController _optionsScreenAnimationController;
        [SerializeField] private MenuScreenAnimationController _audioScreenAnimationController;
        [SerializeField] private MenuScreenAnimationController _videoScreenAnimationController;

        private MenuState _currentState;
        private MenuScreenAnimationController _currentAnimationController;

        private bool _isDetectingInput;

        private void Awake()
        {
            _currentState = MenuState.TITLE;
            _currentAnimationController = null;
            _isDetectingInput = false;

            InputController.GamePlay.InputEnabled = false;
            InputController.GamePlay.MouseEnabled = false;

            LoadingView.instance.FadeOut(null, VariablesManager.uiVariables.defaultFadeInSpeed);

            _titleAnimationHandler.OnFadeEnd = HandleInputEnabled;
            _titleAnimationHandler.OnPressButton = HandleAnyButtonPressed;

            SaveGameManager.instance.Initialize();
            AudioManager.instance.PlayMusic(AudioNameEnum.SOUND_TRACK_INTRO);
        }

        private void Update()
        {
            if (_isDetectingInput && Input.anyKeyDown)
            {
                _titleAnimator.Play("PressedButton");
                _isDetectingInput = false;
            }
        }

        private void HandleInputEnabled()
        {
            _isDetectingInput = true;
        }

        private void HandleAnyButtonPressed()
        {
            ChangeToScreen(MenuState.MAIN_MENU);
        }

        [UsedImplicitly]
        public void OpenSlots()
        {
            ChangeToScreen(MenuState.SLOT_SCREEN);
        }

        [UsedImplicitly]
        public void OpenMainMenu()
        {
            ChangeToScreen(MenuState.MAIN_MENU);
        }

        [UsedImplicitly]
        public void OpenOptions()
        {
            ChangeToScreen(MenuState.OPTIONS_SCREEN);
        }

        [UsedImplicitly]
        public void OpenAudioOptions()
        {
            ChangeToScreen(MenuState.AUDIO_SCREEN);
        }

        [UsedImplicitly]
        public void OpenVideoOptions()
        {
            ChangeToScreen(MenuState.VIDEO_SCREEN);
        }

        [UsedImplicitly]
        public void CloseGame()
        {
            Application.Quit();
        }

        private void ChangeToScreen(MenuState p_nextScreen)
        {
            _currentState = p_nextScreen;

            switch(_currentState)
            {
                case MenuState.MAIN_MENU:
                {
                    if (_currentAnimationController == null)
                        _mainMenuAnimationController.Show(delegate ()
                        {
                            _currentAnimationController = _mainMenuAnimationController;
                        });
                    else
                        HandleScreenTransition(_mainMenuAnimationController);
                    break;
                }
                case MenuState.SLOT_SCREEN:
                {
                    HandleScreenTransition(_slotScreenAnimationController);
                    break;
                }
                case MenuState.OPTIONS_SCREEN:
                {
                    HandleScreenTransition(_optionsScreenAnimationController);
                    break;
                }
                case MenuState.AUDIO_SCREEN:
                {
                    HandleScreenTransition(_audioScreenAnimationController);
                    break;
                }
                case MenuState.VIDEO_SCREEN:
                {
                    HandleScreenTransition(_videoScreenAnimationController);
                    break;
                }
            }
        }

        private void HandleScreenTransition(MenuScreenAnimationController p_nextScreen)
        {
            _currentAnimationController.Hide(delegate ()
            {
                p_nextScreen.Show(delegate ()
                {
                    _currentAnimationController = p_nextScreen;
                });
            });
        }
    }
}
