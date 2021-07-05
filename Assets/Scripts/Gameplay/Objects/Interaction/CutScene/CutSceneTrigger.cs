using CoreEvent.GameEvents;
using GameManagers;
using UnityEngine;
using Utilities;

namespace Gameplay.Objects.Interaction
{
    public class CutSceneTrigger : TriggerColliderController
    {
        private bool _hasRun;
        public bool hasRun { get { return _hasRun; } }

        private void Awake()
        {
            _hasRun = false;
            onTriggerEnter = HandleOnTriggerEnter;
        }

        private void HandleOnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameInternalTags.PLAYER) && !_hasRun)
            {
                _hasRun = true;
                GameEventManager.RunGameEvent(GameEventTypeEnum.CUTSCENE_END_ACT_1);
            }
        }
    }
}