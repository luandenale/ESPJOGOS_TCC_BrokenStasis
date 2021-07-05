using Utilities;

namespace Gameplay.Enemy.EnemiesBase
{
    public interface IEnemyAI : IUpdateBehaviour
    {
        void InitializeEnemy();

        void ResetEnemyAI();
    }
}
