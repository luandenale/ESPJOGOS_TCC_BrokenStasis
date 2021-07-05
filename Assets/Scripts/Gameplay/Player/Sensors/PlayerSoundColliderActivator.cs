using Gameplay.Player.Motion;
using UnityEngine;

namespace Gameplay.Player.Sensors
{
    public class PlayerSoundColliderActivator
    {
        #region PRIVATE READONLY FIELDS
        private readonly Collider _lowSoundCollider;
        private readonly GameObject _lowSoundShader;
        public readonly Collider _mediumSoundCollider;
        private readonly GameObject _mediumSoundShader;
        public readonly Collider _loudSoundCollider;
        private readonly GameObject _loudSoundShader;
        #endregion PRIVATE READONLY FIELDS

        public PlayerSoundColliderActivator(Collider p_lowSoundCollider,
                                            GameObject p_lowSoundShader,
                                            Collider p_mediumSoundCollider,
                                            GameObject p_mediumSoundShader,
                                            Collider p_loudSoundCollider,
                                            GameObject p_loudSoundShader)
        {
            _lowSoundCollider = p_lowSoundCollider;
            _lowSoundShader = p_lowSoundShader;

            _mediumSoundCollider = p_mediumSoundCollider;
            _mediumSoundShader = p_mediumSoundShader;
            
            _loudSoundCollider = p_loudSoundCollider;
            _loudSoundShader = p_loudSoundShader;

            PlayerStatesManager.onStateChanged += HandleStateChanged;
        }

        private void HandleStateChanged(PlayerState p_playerState)
        {
            switch (p_playerState)
            {
                case PlayerState.RUNNING_FORWARD:
                case PlayerState.RUNNING_SIDEWAYS:
                    EnableMediumSoundCollider();
                    break;
                case PlayerState.WALKING_FORWARD:
                case PlayerState.WALKING_SIDEWAYS:
                case PlayerState.WALKING_BACKWARD:
                    EnableLowSoundEmanation();
                    break;
                case PlayerState.STATIC:
                default:
                    DisableAllSoundColliders();
                    break;
            }
        }

        private void EnableLowSoundEmanation()
        {
            DisableAllSoundColliders();
            _lowSoundCollider.enabled = true;
            _lowSoundShader.SetActive(true);
        }

        private void EnableMediumSoundCollider()
        {
            DisableAllSoundColliders();
            _mediumSoundCollider.enabled = true;
            _mediumSoundShader.SetActive(true);
        }

        private void EnableLoudSoundCollider()
        {
            DisableAllSoundColliders();
            _loudSoundCollider.enabled = true;
            _loudSoundShader.SetActive(true);
        }

        private void DisableAllSoundColliders()
        {
            _lowSoundCollider.enabled = false;
            _lowSoundShader.SetActive(false);
            _mediumSoundCollider.enabled = false;
            _mediumSoundShader.SetActive(false);
            _loudSoundCollider.enabled = false;
            _loudSoundShader.SetActive(false);
        }
    }
}
