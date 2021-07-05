using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI.Options.Audio
{
    public class AudioOptionsController : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;

        private void Start()
        {
            _masterVolumeSlider.onValueChanged.AddListener(HandleMasterVolumeChange);
            _sfxVolumeSlider.onValueChanged.AddListener(HandleSFXVolumeChange);
            _musicVolumeSlider.onValueChanged.AddListener(HandleMusicVolumeChange);

            InitializeSlideBars();
        }

        private void InitializeSlideBars()
        {
            _masterVolumeSlider.value = 1f;
            _sfxVolumeSlider.value = 1f;
            _musicVolumeSlider.value = 1f;
            
            if(PlayerPrefs.HasKey(PlayerPrefsSettingsConsts.MASTER_VOLUME))
            {
                _masterVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsSettingsConsts.MASTER_VOLUME);
                _sfxVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsSettingsConsts.SFX_VOLUME);
                _musicVolumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefsSettingsConsts.MUSIC_VOLUME);
            }
        }

        private void HandleMasterVolumeChange(float p_value)
        {
            _audioMixer.SetFloat(PlayerPrefsSettingsConsts.MASTER_VOLUME, ConvertSliderValueToVolume(p_value));
        }

        private void HandleSFXVolumeChange(float p_value)
        {
            _audioMixer.SetFloat(PlayerPrefsSettingsConsts.SFX_VOLUME, ConvertSliderValueToVolume(p_value));
        }

        private void HandleMusicVolumeChange(float p_value)
        {
            _audioMixer.SetFloat(PlayerPrefsSettingsConsts.MUSIC_VOLUME, ConvertSliderValueToVolume(p_value));
        }

        private float ConvertSliderValueToVolume(float p_sliderValue)
        {
            return Mathf.Log10(p_sliderValue) * 20;
        }

        [UsedImplicitly]
        public void SaveChanges()
        {
            PlayerPrefs.SetFloat(PlayerPrefsSettingsConsts.MASTER_VOLUME, _masterVolumeSlider.value);
            PlayerPrefs.SetFloat(PlayerPrefsSettingsConsts.SFX_VOLUME, _sfxVolumeSlider.value);
            PlayerPrefs.SetFloat(PlayerPrefsSettingsConsts.MUSIC_VOLUME, _musicVolumeSlider.value);
        }
    }
}
