using UnityEngine;

namespace Gameplay.Enemy.Behaviours
{
    public interface IAttackMeleeEnemy
    {
        bool CanAttack(Vector3 p_playerPosition);
        void RunUpdate();
    }
}
