using System;
using UnityEngine;
using Utilities;

namespace Gameplay.Enemy.Sensors
{
    public class SensorNoise : MonoBehaviour
    {
        public Action<Transform> onPlayerDetected;
        public Action<Transform> onPlayerRemainsDetected;
        public Action<Transform> onPlayerLeftDetection;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameInternalTags.PLAYER_SOUND_COLLIDER))
                if (onPlayerDetected != null) onPlayerDetected(other.transform);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(GameInternalTags.PLAYER_SOUND_COLLIDER))
                if (onPlayerRemainsDetected != null) onPlayerRemainsDetected(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(GameInternalTags.PLAYER_SOUND_COLLIDER))
                if (onPlayerLeftDetection != null) onPlayerLeftDetection(other.transform);
        }
    }
}
