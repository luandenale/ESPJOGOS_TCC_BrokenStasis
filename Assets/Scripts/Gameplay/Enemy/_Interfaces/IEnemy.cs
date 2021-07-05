using System;
using Utilities;

namespace Gameplay.Enemy
{
    public interface IEnemy : IUpdateBehaviour
    {
        void InitializeEnemy();
    }
}