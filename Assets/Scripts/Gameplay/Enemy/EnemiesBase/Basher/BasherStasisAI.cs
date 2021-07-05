using System;
using GameManagers;
using Gameplay.Enemy.EnemyState;
using Gameplay.Enemy.Sensors;
using UnityEngine;
using Utilities.Audio;

namespace Gameplay.Enemy.EnemiesBase
{
    public class BasherStasisAI : IEnemyAI
    {
        private readonly IEnemyAI _basherAI;
        private readonly SensorVision _stasisSensor;
        private bool _isActive = false;

        public BasherStasisAI(IEnemyAI p_basherAI, SensorVision p_stasisSensor)
        {
            _basherAI = p_basherAI;
            _stasisSensor = p_stasisSensor;
        }

        public void InitializeEnemy()
        {
            _isActive = false;
            _stasisSensor.onPlayerDetected += HandleEnemyActivation;
            _stasisSensor.onPlayerRemainsDetected += HandleEnemyActivation;
        }

        public void ResetEnemyAI()
        {
            _basherAI.ResetEnemyAI();
        }

        public void RunUpdate()
        {
            if (_isActive)
                _basherAI.RunUpdate();
        }

        private void HandleEnemyActivation(Transform p_playerPosition)
        {
            if (!_isActive)
            {
                AudioManager.instance.Play(AudioNameEnum.ENEMY_BASHER_SCREAM);
                _basherAI.InitializeEnemy();
                _isActive = true;
            }
        }
    }
}
