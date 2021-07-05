using Gameplay.Enemy.Sensors;
using UnityEngine;

namespace Gameplay.Enemy.EnemiesBase.Utility
{
    public class RoomTeleportChecker
    {
        private readonly SensorRoom _roomSensor;
        private readonly GameObject[] _roomsToIgnore;

        private bool _shouldTeleport;

        public RoomTeleportChecker(SensorRoom p_roomSensor, GameObject[] p_roomsToIgnore)
        {
            _roomSensor = p_roomSensor;
            _roomsToIgnore = p_roomsToIgnore;
        }

        public void InitializeChecker()
        {
            _shouldTeleport = false;
            _roomSensor.onRoomDetected += HandleEnteredRoom;
        }

        private void HandleEnteredRoom(GameObject p_room)
        {
            foreach(GameObject __room in _roomsToIgnore)
            {
                if(__room == p_room)
                {
                    _shouldTeleport = false;
                    return;
                }
            }
            _shouldTeleport = true;
        }

        public bool ShouldTeleport()
        {
            return _shouldTeleport;
        }
    }
}
