using System;
using Gameplay.Player.Item;
using UnityEngine;

namespace SaveSystem.Player
{
    [Serializable]
    public struct PlayerSaveData
    {
        public int health;
        public Vector3 position;
        public PlayerSuitEnum suit;
        public PlayerIlluminationSaveData playerIlluminationState;
    }
}
