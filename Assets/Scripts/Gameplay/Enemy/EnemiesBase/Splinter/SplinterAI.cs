using GameManagers;
using Gameplay.Enemy.EnemyState;
using Gameplay.Enemy.Sensors;
using UnityEngine;
using Utilities.Audio;

namespace Gameplay.Enemy.EnemiesBase
{
    public class SplinterAI : IEnemyAI
    {
        private readonly IEnemyAI _baseAI;
        private readonly SensorVision _activationSensor;

        private readonly EnemyStateManager _stateManager;
        private readonly EnemyAnimationEventHandler _animationEventHandler;
        private readonly Vector3 _splinterPosition;
        private bool _isActive = false;
        private bool _isAwoken = false;

        public SplinterAI(IEnemyAI p_basherAI,
                            SensorVision p_stasisSensor,
                            EnemyStateManager p_stateManager,
                            EnemyAnimationEventHandler p_animationEventHandler,
                            Vector3 p_splinterPosition)
        {
            _baseAI = p_basherAI;
            _activationSensor = p_stasisSensor;
            _stateManager = p_stateManager;
            _animationEventHandler = p_animationEventHandler;
            _splinterPosition = p_splinterPosition;
        }

        public void InitializeEnemy()
        {
            _isActive = false;
            _isAwoken = false;
            _activationSensor.onPlayerDetected += HandleEnemyActivation;
            _activationSensor.onPlayerRemainsDetected += HandleEnemyActivation;
            _animationEventHandler.OnAwoken += HandleEnemyAwaken;
            _animationEventHandler.OnGrowl += HandleEnemyGrowl;
        }

        public void ResetEnemyAI()
        {
            _baseAI.ResetEnemyAI();
        }

        public void RunUpdate()
        {
            if (_isActive && _isAwoken)
            {
                _baseAI.RunUpdate();
            }
        }

        private void HandleEnemyAwaken()
        {
            _isAwoken = true;
            _baseAI.InitializeEnemy();
        }

        private void HandleEnemyGrowl()
        {
            AudioManager.instance.Play(AudioNameEnum.ENEMY_SPLINTER_GROWL, false, null, false);
        }

        private void HandleEnemyActivation(Transform p_playerPosition)
        {
            if (!_isActive)
            {
                _stateManager.SetEnemyState(EnemyStateEnum.AWAKENING);
                _activationSensor.gameObject.SetActive(false);
                AudioManager.instance.PlayMusic(AudioNameEnum.SOUND_TRACK_SPLINTER, 5.0f);

                _isActive = true;
            }
        }
    }
}
