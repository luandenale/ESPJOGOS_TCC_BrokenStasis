using System;
using UnityEngine;

namespace UI.MainMenu
{
    [RequireComponent(typeof(Animator))]
    public class TitleMenuAnimationEventHandler : MonoBehaviour
    {
        public Action OnFadeEnd;
        public Action OnPressButton;

        public void HandleAnimationEvent(TitleMenuAnimationEventEnum p_eventName)
        {
            switch (p_eventName)
            {
                case TitleMenuAnimationEventEnum.ON_FADE_END:
                    OnFadeEnd?.Invoke();
                    break;
                case TitleMenuAnimationEventEnum.ON_PRESS_BUTTON:
                    OnPressButton?.Invoke();
                    break;
            }
        }
    }
}
