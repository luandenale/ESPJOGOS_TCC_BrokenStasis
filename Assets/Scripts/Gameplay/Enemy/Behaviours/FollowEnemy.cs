using Gameplay.Enemy.EnemyState;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemy.Behaviours
{
    public class FollowEnemy : IFollowEnemy
    {
        private readonly EnemyStateManager _stateManager;
        private readonly NavMeshAgent _navigationAgent;
        private readonly float _investigateSpeedMultiplier;
        private readonly float _sprintSpeedMultiplier;

        private float _initialSpeed;
        private float _initialAcceleration;
        private float _initialAngularSpeed;

        public FollowEnemy(
            EnemyStateManager p_stateManager,
            NavMeshAgent p_navigationAgent,
            float p_investigateSpeedMultiplier,
            float p_sprintSpeedMultiplier)
        {
            _stateManager = p_stateManager;
            _navigationAgent = p_navigationAgent;
            _investigateSpeedMultiplier = p_investigateSpeedMultiplier;
            _sprintSpeedMultiplier = p_sprintSpeedMultiplier;

            _initialSpeed = _navigationAgent.speed;
            _initialAcceleration = _navigationAgent.acceleration;
            _initialAngularSpeed = _navigationAgent.angularSpeed;

            InitializeFollowBehaviour();
        }

        private void InitializeFollowBehaviour()
        {
            _stateManager.onStateChanged += HandleStateChanged;
        }

        private void HandleStateChanged(EnemyStateEnum p_enemyState)
        {
            switch (p_enemyState)
            {
                case EnemyStateEnum.INVESTIGATING:
                    _navigationAgent.speed = _initialSpeed * _investigateSpeedMultiplier;
                    _navigationAgent.acceleration = _initialAcceleration * _investigateSpeedMultiplier;
                    _navigationAgent.angularSpeed = _initialAngularSpeed * _investigateSpeedMultiplier;
                    break;
                case EnemyStateEnum.RUNNING:
                    _navigationAgent.speed = _initialSpeed * _sprintSpeedMultiplier;
                    _navigationAgent.acceleration = _initialAcceleration * _sprintSpeedMultiplier;
                    _navigationAgent.angularSpeed = _initialAngularSpeed * _sprintSpeedMultiplier;
                    break;
                case EnemyStateEnum.ATTACKING:
                    StopNavigation();
                    break;
                default:
                    break;
            }
        }

        public void RunFollowEnemy()
        {
            if(!_navigationAgent.isOnNavMesh) return;
            if (IsEnemyFollowing() && _navigationAgent.remainingDistance < 0.05f)
                _stateManager.SetEnemyState(EnemyStateEnum.INVESTIGATING_IDLE);
        }

        private bool IsEnemyFollowing()
        {
            return (_stateManager.currentState == EnemyStateEnum.RUNNING || _stateManager.currentState == EnemyStateEnum.INVESTIGATING);
        }

        public void InvestigatePosition(Transform p_destinationPosition)
        {
            SetNavigationDestination(p_destinationPosition);

            if (_stateManager.currentState != EnemyStateEnum.RUNNING)
                _stateManager.SetEnemyState(EnemyStateEnum.INVESTIGATING);
        }

        public void SprintToPosition(Transform p_destinationPosition)
        {
            SetNavigationDestination(p_destinationPosition);

            _stateManager.SetEnemyState(EnemyStateEnum.RUNNING);
        }

        private void StopNavigation()
        {
            _navigationAgent.isStopped = true;
        }

        private void SetNavigationDestination(Transform p_destinationPosition)
        {
            _navigationAgent.isStopped = false;

            _navigationAgent.SetDestination(p_destinationPosition.position);
        }
    }
}