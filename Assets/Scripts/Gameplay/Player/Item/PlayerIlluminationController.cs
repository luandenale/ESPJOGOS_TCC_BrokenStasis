using GameManagers;
using SaveSystem.Player;
using UnityEngine;
using Utilities.Audio;

namespace Gameplay.Player.Item
{

    public class PlayerIlluminationController
    {
        private readonly GameObject _illuminationObject;
        private PlayerIlluminationSaveData _illuminationState;

        public PlayerIlluminationController(GameObject p_illumination)
        {
            _illuminationObject = p_illumination;
            _illuminationObject.SetActive(false);
        }

        public PlayerIlluminationSaveData lanternState
        {
            get
            {
                return _illuminationState;
            }

            set
            {
                _illuminationState = value;

                _illuminationObject.SetActive(_illuminationState.toggledOn);
            }
        }

        public void SetActive(bool p_active)
        {
            _illuminationState.enabled = p_active;
        }

        public void Toggle()
        {
            if (_illuminationState.enabled && GameStateManager.currentState == GameState.RUNNING)
            {
                _illuminationState.toggledOn = !_illuminationState.toggledOn;
                AudioManager.instance.Play(AudioNameEnum.ITEM_LANTERN_CLICK);
                _illuminationObject.SetActive(_illuminationState.toggledOn);
            }
        }
    }
}