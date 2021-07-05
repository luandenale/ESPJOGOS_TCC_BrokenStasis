using GameManagers;
using Gameplay.Player.Motion;
using UnityEngine;
using Utilities;
using Utilities.Audio;
using Utilities.VariableManagement;

namespace Gameplay.Player.Health
{
    public class PlayerHealth
    {
        private int _playerMaxHealth;
        private int _playerCurrentHealth;
        private PlayerHealthStateEnum _playerHealthState;
        private AudioSource _heartBeatAudio;

        public PlayerHealth()
        {
            if (VariablesManager.playerVariables.maxHealth <= 0)
                throw new System.Exception("Player max health variable is " + VariablesManager.playerVariables.maxHealth + "Minimum of life is 1.");

            this._playerMaxHealth = VariablesManager.playerVariables.maxHealth;
            this._playerCurrentHealth = this._playerMaxHealth;

            _playerHealthState = (PlayerHealthStateEnum)_playerCurrentHealth;

            HandleHeartBeat();
        }

        public void ReceiveDamage(int p_damage)
        {
            if(!InputController.GamePlay.InputEnabled || GameStateManager.currentState == GameState.CUTSCENE || VariablesManager.playerVariables.isPlayerInvencible) return;
            
            if (_playerCurrentHealth - p_damage < 0)
                _playerCurrentHealth = 0;
            else
                _playerCurrentHealth -= p_damage;

            if (_playerCurrentHealth == 0)
                HandlePlayerDeath();
            else
            {
                AudioManager.instance.Play(AudioNameEnum.PLAYER_HIT, false, null, true);
                GameHudManager.instance.damageUI.ReceiveHit(_playerCurrentHealth);
                PlayerStatesManager.SetPlayerState(PlayerState.HIT);
            }

            _playerHealthState = (PlayerHealthStateEnum)_playerCurrentHealth;

            HandleHeartBeat();
        }

        public void SetPlayerHealth(int p_health)
        {
            _playerCurrentHealth = p_health;
            _playerHealthState = (PlayerHealthStateEnum)_playerCurrentHealth;

            HandleHeartBeat();
        }

        public void IncreaseHealth(int p_lifePoints)
        {
            _playerCurrentHealth = p_lifePoints;

            if (_playerCurrentHealth > _playerMaxHealth)
                _playerCurrentHealth = _playerMaxHealth;

            _playerHealthState = (PlayerHealthStateEnum)_playerCurrentHealth;

            HandleHeartBeat();
        }

        public int GetPlayerHealth()
        {
            return _playerCurrentHealth;
        }

        private void HandleHeartBeat()
        {
            if(_playerHealthState == PlayerHealthStateEnum.FINE) return;

            if (_heartBeatAudio == null || !_heartBeatAudio.isPlaying)
                _heartBeatAudio = AudioManager.instance.Play(AudioNameEnum.PLAYER_HEARTBEAT, true);

            switch (_playerHealthState)
            {
                case PlayerHealthStateEnum.FINE:
                    _heartBeatAudio.pitch = 1f;
                    break;
                
                case PlayerHealthStateEnum.DANGER:
                    _heartBeatAudio.pitch = 1.25f;
                    break;
                case PlayerHealthStateEnum.CRITICAL:
                    _heartBeatAudio.pitch = 1.5f;
                    break;
                case PlayerHealthStateEnum.DEAD:
                    _heartBeatAudio.Stop();
                    break;
                default:
                    break;
            }
        }

        private void HandlePlayerDeath()
        {
            AudioManager.instance.Play(AudioNameEnum.PLAYER_DIE);

            PlayerStatesManager.SetPlayerState(PlayerState.DEAD);
            GameStateManager.SetGameState(GameState.GAMEOVER);
            GameHudManager.instance.gameOverUI.StartUIHandlers();
        }

        public PlayerHealthStateEnum GetPlayerHealthState()
        {
            return _playerHealthState;
        }
    }
}
