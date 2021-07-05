using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using GameManagers;
using Gameplay.Enemy.EnemyState;

namespace Gameplay.Enemy.Behaviours
{
    public class PatrolEnemy : IPatrolEnemy
    {
        private readonly EnemyStateManager _stateManager;
        private readonly NavMeshAgent _navigationAgent;
        private readonly GameObject _patrolPointsGameObject;
        private readonly float _idleTime;
        private readonly float _patrolSpeedMultiplier;

        private List<Transform> _patrolPointsList;
        private int _patrolIndex;
        private bool _settingDestination;

        private float _initialSpeed;
        private float _initialAcceleration;
        private float _initialAngularSpeed;

        private Coroutine _patrolCoroutine;

        public PatrolEnemy(
            EnemyStateManager p_stateManager,
            NavMeshAgent p_navigationAgent,
            GameObject p_patrolPointsGameObject,
            float p_idleTime,
            float p_patrolSpeedMultiplier)
        {
            _stateManager = p_stateManager;
            _navigationAgent = p_navigationAgent;
            _patrolPointsGameObject = p_patrolPointsGameObject;
            _idleTime = p_idleTime;
            _patrolSpeedMultiplier = p_patrolSpeedMultiplier;
        }

        public void InitializePatrolBehaviour()
        {
            _patrolIndex = 0;
            _settingDestination = false;
            _patrolPointsList = _patrolPointsGameObject.GetComponentsInChildren<Transform>().ToList();
            _patrolPointsList.Remove(_patrolPointsGameObject.transform);

            _stateManager.onStateChanged += HandleStateChanged;

            _initialSpeed = _navigationAgent.speed;
            _initialAcceleration = _navigationAgent.acceleration;
            _initialAngularSpeed = _navigationAgent.angularSpeed;

            _patrolCoroutine = CustomSceneManager.instance.StartCoroutine(PatrolToNextPoint());
        }

        private void HandleStateChanged(EnemyStateEnum p_enemyState)
        {
            switch (p_enemyState)
            {
                case EnemyStateEnum.IDLE:
                    ResetSpeed();
                    break;
                case EnemyStateEnum.PATROLLING:
                    if (_navigationAgent.remainingDistance < 0.05f)
                        SetPatrolDestination();
                    break;
                case EnemyStateEnum.INVESTIGATING:
                case EnemyStateEnum.RUNNING:
                case EnemyStateEnum.INVESTIGATING_ROOM:
                case EnemyStateEnum.INVESTIGATING_IDLE:
                    StopPatrolling();
                    break;
                default:
                    break;
            }
        }

        public void RunEnemyPatrol()
        {
            if (!IsEnemyPatrolling() || (_navigationAgent.isOnNavMesh && _navigationAgent.remainingDistance < 0.05f))
                if (!_settingDestination)
                {
                    CustomSceneManager.instance.StopCoroutine(_patrolCoroutine);
                    _patrolCoroutine = CustomSceneManager.instance.StartCoroutine(PatrolToNextPoint());
                }
        }

        private bool IsEnemyPatrolling()
        {
            return (_stateManager.currentState == EnemyStateEnum.PATROLLING || _stateManager.currentState == EnemyStateEnum.IDLE);
        }

        private IEnumerator PatrolToNextPoint()
        {
            _settingDestination = true;

            _stateManager.SetEnemyState(EnemyStateEnum.IDLE);

            yield return new WaitForSeconds(_idleTime);

            _stateManager.SetEnemyState(EnemyStateEnum.PATROLLING);

            _navigationAgent.isStopped = false;

            _settingDestination = false;
        }

        private void StopPatrolling()
        {
            _settingDestination = false;
            CustomSceneManager.instance.StopCoroutine(_patrolCoroutine);
        }

        private void SetPatrolDestination()
        {
            _navigationAgent.SetDestination(_patrolPointsList[_patrolIndex].transform.position);

            if (_patrolIndex == _patrolPointsList.Count - 1)
                _patrolIndex = 0;
            else
                _patrolIndex++;
        }

        private void ResetSpeed()
        {
            _navigationAgent.speed = _initialSpeed * _patrolSpeedMultiplier;
            _navigationAgent.acceleration = _initialAcceleration * _patrolSpeedMultiplier;
            _navigationAgent.angularSpeed = _initialAngularSpeed * _patrolSpeedMultiplier;
        }
    }
}