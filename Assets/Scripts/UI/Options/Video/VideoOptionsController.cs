using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Options.Video
{
    public class VideoOptionsController : MonoBehaviour
    {
        [SerializeField] private MenuSceneController _menuSceneController;
        [SerializeField] private VideoConfirmationPopUp _confirmationPopUp;
        [SerializeField] private MultiOption _resolutionMultiOption;
        [SerializeField] private Toggle _fullScreenToggle;
        [SerializeField] private Button _applyReturnButton;
        [SerializeField] private TextMeshProUGUI _applyReturnText;

        private bool _hasChanges;

        private bool _isFullscreen;
        private string _currentResolution;

        private void Start()
        {    
            LoadSettings();
            InitializeVideoOptions();
        }

        [UsedImplicitly]
        public void InitializeVideoOptions()
        {
            InitializeMultiOption();
            InitializeToggle();

            OptionsChanged(false);
        }

        private void LoadSettings()
        {
            _isFullscreen = Screen.fullScreen;
            _currentResolution = Screen.width + " x " + Screen.height;
            
            if(PlayerPrefs.HasKey(PlayerPrefsSettingsConsts.FULLSCREEN_TOGGLE))
            {
                _isFullscreen = PlayerPrefs.GetInt(PlayerPrefsSettingsConsts.FULLSCREEN_TOGGLE) == 1 ? true : false;
                _currentResolution = PlayerPrefs.GetString(PlayerPrefsSettingsConsts.SCREEN_RESOLUTION);
            }
        }

        private void InitializeToggle()
        {
            _fullScreenToggle.isOn = _isFullscreen;
            _fullScreenToggle.onValueChanged.AddListener(HandleFullscreenToggleChanged);
        }

        private void HandleFullscreenToggleChanged(bool p_toggle)
        {
            if(_isFullscreen != p_toggle || _currentResolution != _resolutionMultiOption.GetCurrentOption())
                OptionsChanged(true);
            else
                OptionsChanged(false);
        }

        private void InitializeMultiOption()
        {
            List<string> __availableResolutions = new List<string>();
            
            foreach(Resolution __resolution in Screen.resolutions)
            {
                string __parsedResolution = __resolution.width + " x " + __resolution.height;

                if(!__availableResolutions.Contains(__parsedResolution))
                    __availableResolutions.Add(__parsedResolution);
            }

            _resolutionMultiOption.InitializeOptions(_currentResolution, __availableResolutions.ToArray());

            _resolutionMultiOption.onOptionChanged = HandleMultiOptionChanged;
        }

        private void HandleMultiOptionChanged()
        {
            if(_currentResolution != _resolutionMultiOption.GetCurrentOption() || _isFullscreen != Screen.fullScreen)
                OptionsChanged(true);
            else
                OptionsChanged(false);
        }

        private void OptionsChanged(bool p_apply)
        {
            if(p_apply)
            {
                _applyReturnText.alpha = 1f;
                _applyReturnButton.interactable = true;
            }
            else
            {
                _applyReturnText.alpha = 0.5f;
                _applyReturnButton.interactable = false;
            }
        }

        [UsedImplicitly]
        public void PressedApply()
        {
            SetResolution(_resolutionMultiOption.GetCurrentOption(), _fullScreenToggle.isOn);
            
            _confirmationPopUp.StartUIHandlers(ApplySettings, RevertChanges);
        }

        private void ApplySettings()
        {
            _currentResolution = _resolutionMultiOption.GetCurrentOption();
            _isFullscreen = _fullScreenToggle.isOn;
            
            PlayerPrefs.SetString(PlayerPrefsSettingsConsts.SCREEN_RESOLUTION, _currentResolution);
            PlayerPrefs.SetInt(PlayerPrefsSettingsConsts.FULLSCREEN_TOGGLE, (_isFullscreen == true ? 1 : 0));

            OptionsChanged(false);

            _menuSceneController.OpenOptions();
        }

        private void RevertChanges()
        {
            _resolutionMultiOption.SetOption(_currentResolution);
            _fullScreenToggle.isOn = _isFullscreen;

            OptionsChanged(false);

            SetResolution(_currentResolution, _isFullscreen);
        }

        private void SetResolution(string p_resolution, bool p_fullScreen)
        {
            Screen.SetResolution(Int32.Parse(p_resolution.Substring(0, p_resolution.IndexOf(" "))),
                                    Int32.Parse(p_resolution.Substring(p_resolution.IndexOf(" x ") + 3)),
                                    p_fullScreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed);
        }
    }
}