using System;
using UnityEngine;

namespace UI.MainMenu
{
    public class MenuScreenAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private UIAnimationEventHandler _animationEventHandler;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animationEventHandler = GetComponent<UIAnimationEventHandler>();
        }

        public void Show(Action p_onShow = null)
        {
            _animationEventHandler.OnShowAnimationEnd = p_onShow;
            _animator.Play("Show");
        }
        
        public void Hide(Action p_onHide = null)
        {
            _animationEventHandler.OnHideAnimationEnd = p_onHide;
            _animator.Play("Hide");
        }
    }
}
