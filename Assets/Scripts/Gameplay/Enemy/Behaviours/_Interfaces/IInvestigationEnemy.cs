using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Enemy.Behaviours
{
    public interface IInvestigationEnemy
    {
        void InitializeInvestigationBehaviour();
        void RunEnemyInvestigation();
        void SetInvestigationPoints(List<Transform> p_investigationPoints);
    }
}