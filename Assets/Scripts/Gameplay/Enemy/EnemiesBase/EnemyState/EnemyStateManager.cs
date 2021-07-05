using System;

namespace Gameplay.Enemy.EnemyState
{
    public class EnemyStateManager
    {
        public EnemyStateEnum currentState { get; private set; }

        public Action<EnemyStateEnum> onStateChanged;

        public void SetEnemyState(EnemyStateEnum p_newState)
        {
            if (!currentState.Equals(p_newState))
            {
                currentState = p_newState;

                if (onStateChanged != null) onStateChanged(currentState);
            }
        }
    }
}
