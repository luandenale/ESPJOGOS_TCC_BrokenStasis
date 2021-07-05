using System;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class UIAnimationEventHandler : MonoBehaviour
    {
        public Action OnShowAnimationEnd;
        public Action OnHideAnimationEnd;

        public void HandleAnimationEvent(UIAnimationEventEnum p_eventName)
        {
            switch (p_eventName)
            {
                case UIAnimationEventEnum.ON_SHOW_END:
                    OnShowAnimationEnd?.Invoke();
                    break;
                case UIAnimationEventEnum.ON_HIDE_END:
                    OnHideAnimationEnd?.Invoke();
                    break;
            }
        }
    }
}
