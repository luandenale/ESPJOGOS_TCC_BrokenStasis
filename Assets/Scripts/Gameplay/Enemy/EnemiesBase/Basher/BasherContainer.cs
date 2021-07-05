using Gameplay.Enemy.Sensors;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemy.EnemiesBase
{
    public class BasherContainer : MonoBehaviour
    {
        public NavMeshAgent navigationAgent;

        [Header("Basher Type")]
        [Space(5)]
        public BasherTypeEnum basherType;

        [Header("Animator References")]
        [Space(5)]
        public Animator animator;
        public EnemyAnimationEventHandler enemyAnimationEventHandler;

        [Header("Patrol Variables")]
        [Space(5)]
        public GameObject patrolPointsGameObject;
        public float idleTime = 0.75f;
        public float patrolSpeedMultiplier = 0.5f;

        [Header("Follow Variables")]
        [Space(5)]
        public float investigateSpeedMultiplier = 0.75f;
        public float sprintSpeedMultiplier = 1f;

        [Header("Room Investigation Variables")]
        [Space(5)]
        public float roomInvestigationSpeedMultiplier = 0.75f;

        [Header("Attack Variables")]
        [Space(5)]
        public SensorDamagePlayer weaponSensor;
        public BoxCollider attackArea;
        public Transform originPosition;
        public float attackRange = 1.75f;
        public int damage = 1;

        [Header("Sensors")]
        [Space(5)]
        public SensorNoise noiseSensor;
        public SensorVision visionSensor;
        public SensorRoom roomSensor;
        public SensorVision stasisSensor;

        [Header("EndGame Door")]
        [Space(5)]
        public Transform endGameSpawnTransform;
        public GameObject[] roomsToIgnoreTeleport;

        private void Awake()
        {
            if (navigationAgent == null)
                throw new MissingComponentException("NavigationAgent not found in BasherContainer!");
            if (animator == null)
                throw new MissingComponentException("Animator not found in BasherContainer!");
            if (enemyAnimationEventHandler == null)
                throw new MissingComponentException("EnemyAnimationEventHandler not found in BasherContainer!");
            if (visionSensor == null)
                throw new MissingComponentException("VisionSensor not found in BasherContainer!");
            if (noiseSensor == null)
                throw new MissingComponentException("NoiseSensor not found in BasherContainer!");
            if (originPosition == null)
                throw new MissingComponentException("OriginPosition not found in BasherContainer!");
            if (weaponSensor == null)
                throw new MissingComponentException("WeaponSensor not found in BasherContainer!");
            if (patrolPointsGameObject == null)
                throw new MissingComponentException("PatrolPointsGameObject not found in BasherContainer!");
        }
    }
}
