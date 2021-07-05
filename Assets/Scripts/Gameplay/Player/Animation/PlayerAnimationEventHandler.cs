using System;
using UnityEngine;

namespace Gameplay.Player.Animation
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationEventHandler : MonoBehaviour
    {
        public Action OnStep;
        public Action OnCutsceneEnd;

        public void HandleAnimationEvent(PlayerAnimationEventEnum p_eventName)
        {
            switch (p_eventName)
            {
                case PlayerAnimationEventEnum.ON_STEP:
                    OnStep?.Invoke();
                    break;
                case PlayerAnimationEventEnum.ON_CUTSCENE_END:
                    OnCutsceneEnd?.Invoke();
                    break;
            }
        }
    }
}