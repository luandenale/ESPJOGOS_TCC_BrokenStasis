using System;
using GameManagers;
using Gameplay.Enemy.EnemyState;
using Gameplay.Enemy.Sensors;
using UnityEngine;
using Utilities;

namespace Gameplay.Enemy.Behaviours
{
    public class AttackMeleeEnemy : IUpdateBehaviour, IAttackMeleeEnemy
    {
        private readonly SensorDamagePlayer _weaponSensor;
        private readonly BoxCollider _attackArea;
        private readonly Transform _originPosition;
        private readonly float _attackRange;
        private readonly int _damage;
        private readonly EnemyStateManager _enemyStatesManager;

        public AttackMeleeEnemy(
            EnemyStateManager p_stateManager,
            SensorDamagePlayer p_weaponSensor,
            Transform p_originPosition,
            float p_attackRange,
            int p_damage,
            BoxCollider p_attackArea
            )
        {
            _enemyStatesManager = p_stateManager;
            _weaponSensor = p_weaponSensor;
            _originPosition = p_originPosition;
            _attackRange = p_attackRange;
            _damage = p_damage;
            _attackArea = p_attackArea;

            InitializeAttackingBehaviour();
        }

        private void InitializeAttackingBehaviour()
        {
            _enemyStatesManager.onStateChanged += HandleStateChanged;
        }

        private void HandleStateChanged(EnemyStateEnum p_enemyState)
        {
            switch (p_enemyState)
            {
                case EnemyStateEnum.ATTACKING:
                    _weaponSensor.ResetSensorDetection();
                    break;
                default:
                    break;
            }
        }

        public bool CanAttack(Vector3 p_playerPosition)
        {
            return (Vector3.Distance(_originPosition.position, p_playerPosition) < _attackRange)
                && GameStateManager.currentState != GameState.GAMEOVER
                && _attackArea.bounds.Contains(p_playerPosition);
        }

        public void RunUpdate()
        {
            if (_weaponSensor.isTouchingPlayer)
            {
                _weaponSensor.ResetSensorDetection();
                GameplayManager.instance.onPlayerDamaged?.Invoke(_damage);
            }
        }
    }
}
