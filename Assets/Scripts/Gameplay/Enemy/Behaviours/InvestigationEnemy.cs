using System.Collections;
using System.Collections.Generic;
using GameManagers;
using Gameplay.Enemy.EnemyState;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemy.Behaviours
{
    // TODO: Refactor class since it shares similar functionalities with PatrolEnemy
    public class InvestigationEnemy : IInvestigationEnemy
    {
        private readonly EnemyStateManager _stateManager;
        private readonly NavMeshAgent _navigationAgent;
        private readonly float _idleTime;
        private readonly float _investigationSpeedMultiplier;

        private List<Transform> _investigationPoints = new List<Transform>();
        private int _investigationIndex;
        private bool _settingDestination;

        private float _initialSpeed;
        private float _initialAcceleration;
        private float _initialAngularSpeed;

        public InvestigationEnemy(
            EnemyStateManager p_stateManager,
            NavMeshAgent p_navigationAgent,
            float p_idleTime,
            float p_investigationSpeedMultiplier)
        {
            _stateManager = p_stateManager;
            _navigationAgent = p_navigationAgent;
            _idleTime = p_idleTime;
            _investigationSpeedMultiplier = p_investigationSpeedMultiplier;
        }

        public void InitializeInvestigationBehaviour()
        {
            _investigationIndex = 0;
            _settingDestination = false;

            _stateManager.onStateChanged += HandleStateChanged;

            _initialSpeed = _navigationAgent.speed;
            _initialAcceleration = _navigationAgent.acceleration;
            _initialAngularSpeed = _navigationAgent.angularSpeed;

            CustomSceneManager.instance.StartCoroutine(InvestigateNextPoint());
        }

        public void SetInvestigationPoints(List<Transform> p_investigationPoints)
        {
            _investigationPoints = p_investigationPoints;
        }

        private void HandleStateChanged(EnemyStateEnum p_enemyState)
        {
            switch (p_enemyState)
            {
                case EnemyStateEnum.INVESTIGATING_IDLE:
                    ResetSpeed();
                    break;
                case EnemyStateEnum.INVESTIGATING_ROOM:
                    SetPatrolDestination();
                    break;
                case EnemyStateEnum.INVESTIGATING:
                case EnemyStateEnum.RUNNING:
                case EnemyStateEnum.IDLE:
                case EnemyStateEnum.PATROLLING:
                    StopInvestigating();
                    break;
                default:
                    break;
            }
        }

        public void RunEnemyInvestigation()
        {
            if (!IsEnemyInvestigating() || _navigationAgent.remainingDistance < 0.05f)
                if (!_settingDestination)
                    CustomSceneManager.instance.StartCoroutine(InvestigateNextPoint());
        }

        private bool IsEnemyInvestigating()
        {
            return (_stateManager.currentState == EnemyStateEnum.INVESTIGATING_ROOM || _stateManager.currentState == EnemyStateEnum.INVESTIGATING_IDLE);
        }

        private IEnumerator InvestigateNextPoint()
        {
            _settingDestination = true;

            _stateManager.SetEnemyState(EnemyStateEnum.INVESTIGATING_IDLE);

            yield return new WaitForSeconds(_idleTime);

            _stateManager.SetEnemyState(EnemyStateEnum.INVESTIGATING_ROOM);

            _navigationAgent.isStopped = false;

            _settingDestination = false;
        }

        private void StopInvestigating()
        {
            _settingDestination = false;
            CustomSceneManager.instance.StopCoroutine(InvestigateNextPoint());
        }

        private void SetPatrolDestination()
        {
            if (_investigationPoints.Count == 0 || _investigationIndex >= _investigationPoints.Count)
            {
                _stateManager.SetEnemyState(EnemyStateEnum.IDLE);
                _investigationIndex = 0;
                return;
            }

            _navigationAgent.SetDestination(_investigationPoints[_investigationIndex].position);

            _investigationIndex++;
        }

        private void ResetSpeed()
        {
            _navigationAgent.speed = _initialSpeed * _investigationSpeedMultiplier;
            _navigationAgent.acceleration = _initialAcceleration * _investigationSpeedMultiplier;
            _navigationAgent.angularSpeed = _initialAngularSpeed * _investigationSpeedMultiplier;
        }
    }
}