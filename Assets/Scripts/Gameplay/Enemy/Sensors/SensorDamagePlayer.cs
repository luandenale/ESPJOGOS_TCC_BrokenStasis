using UnityEngine;
using Utilities;

namespace Gameplay.Enemy.Sensors
{
    public class SensorDamagePlayer : MonoBehaviour
    {
        public bool isTouchingPlayer { private set; get; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(GameInternalTags.PLAYER))
            {
                isTouchingPlayer = true;
            }
        }

        public void ResetSensorDetection()
        {
            isTouchingPlayer = false;
        }
    }
}
