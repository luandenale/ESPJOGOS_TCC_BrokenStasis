using System;
using UnityEngine;

namespace Gameplay.Enemy.Sensors
{
    public class SensorRoom : MonoBehaviour
    {
        [SerializeField] private LayerMask RoomCollisionLayer;
        public Action<GameObject> onRoomDetected;

        private void OnTriggerEnter(Collider other)
        {
            if((RoomCollisionLayer.value & 1<< other.gameObject.layer) == 1 << other.gameObject.layer)
            {
                onRoomDetected?.Invoke(other.transform.parent.gameObject);
            }
        }
    }
}
