using System;
using GameManagers;
using Gameplay.Enemy.Behaviours;
using Gameplay.Enemy.EnemiesBase.Utility;
using Gameplay.Enemy.EnemyState;
using UnityEngine;
using Utilities.Audio;

namespace Gameplay.Enemy.EnemiesBase
{
    public class BasherEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private BasherContainer _basherContainer;

        private EnemyStateManager _stateManager;
        private EnemyAnimator _enemyAnimator;
        private IEnemyAI _basherAI;
        private RoomTeleportChecker _roomTeleportChecker;

        private void Awake()
        {
            if (_basherContainer == null)
                throw new MissingComponentException("BasherContainer not found in BasherEnemy!");
        }

        public void InitializeEnemy()
        {
            RegisterObjectsGraph();

            _basherAI.ResetEnemyAI();
            _basherAI.InitializeEnemy();
            _roomTeleportChecker.InitializeChecker();

            GameStateManager.onStateChanged += HandleGameStateChanged;
        }

        private void RegisterObjectsGraph()
        {
            _stateManager = new EnemyStateManager();

            _enemyAnimator = new EnemyAnimator(
                _stateManager,
                _basherContainer.animator
            );

            _basherAI = new BasherAI(
                _stateManager,
                new PatrolEnemy(
                    _stateManager,
                    _basherContainer.navigationAgent,
                    _basherContainer.patrolPointsGameObject,
                    _basherContainer.idleTime,
                    _basherContainer.patrolSpeedMultiplier
                ),
                new FollowEnemy(
                    _stateManager,
                    _basherContainer.navigationAgent,
                    _basherContainer.investigateSpeedMultiplier,
                    _basherContainer.sprintSpeedMultiplier
                ),
                new InvestigationEnemy(
                    _stateManager,
                    _basherContainer.navigationAgent,
                    _basherContainer.idleTime,
                    _basherContainer.investigateSpeedMultiplier
                ),
                new AttackMeleeEnemy(
                    _stateManager,
                    _basherContainer.weaponSensor,
                    _basherContainer.originPosition,
                    _basherContainer.attackRange,
                    _basherContainer.damage,
                    _basherContainer.attackArea
                ),
                _basherContainer.noiseSensor,
                _basherContainer.visionSensor,
                _basherContainer.roomSensor,
                _basherContainer.enemyAnimationEventHandler,
                _basherContainer.navigationAgent.transform
            );

            _roomTeleportChecker = new RoomTeleportChecker(
                _basherContainer.roomSensor,
                _basherContainer.roomsToIgnoreTeleport
            );

            if (_basherContainer.basherType.Equals(BasherTypeEnum.STASIS))
            {
                _basherAI = new BasherStasisAI(
                    _basherAI,
                    _basherContainer.stasisSensor
                );
            }
            else if (_basherContainer.basherType.Equals(BasherTypeEnum.SPLINTER))
            {
                _basherAI = new SplinterAI(
                    _basherAI,
                    _basherContainer.stasisSensor,
                    _stateManager,
                    _basherContainer.enemyAnimationEventHandler,
                    gameObject.transform.position
                );
            }
        }

        public void RunUpdate()
        {
            _basherAI.RunUpdate();
        }

        public void TeleportToEndgameDoorSpawn(Vector3 p_doorPosition)
        {
            if(_roomTeleportChecker.ShouldTeleport())
            {
                _basherContainer.navigationAgent.Warp(_basherContainer.endGameSpawnTransform.position);
                _basherContainer.navigationAgent.transform.rotation = _basherContainer.endGameSpawnTransform.rotation;
                _basherContainer.navigationAgent?.SetDestination(p_doorPosition);
            }
            else
            {
                _basherContainer.visionSensor.onPlayerDetected(_basherContainer.endGameSpawnTransform);
            }

            AudioManager.instance.PlayAtPosition(AudioNameEnum.ENEMY_SPLINTER_GROWL, _basherContainer.endGameSpawnTransform.position, false, AudioRange.MEDIUM, false, true, _basherContainer.navigationAgent.transform.parent.name);
        }

        private void HandleGameStateChanged(GameState p_gameState)
        {
            switch (p_gameState)
            {
                case GameState.GAMEOVER:
                    _basherAI.ResetEnemyAI();
                    break;
                default:
                    break;
            }
        }
    }
}
