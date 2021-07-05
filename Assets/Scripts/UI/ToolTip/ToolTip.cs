using UnityEngine;
using Utilities;

namespace UI.ToolTip
{
    public class ToolTip : MonoBehaviour
    {
        [SerializeField] private ToolTip _nextTooltip;
        [SerializeField] private bool _isActive = false;

        private Animator _toolTipAnimator;

        private const string INACTIVE_ANIMATION = "Inactive";
        private const string ACTIVE_ANIMATION = "Active";
        private const string SHOW_ANIMATION = "Show";
        private const string HIDE_ANIMATION = "Hide";
        private const string INTERACT_ANIMATION = "Interact";

        private void Awake()
        {
            _toolTipAnimator = GetComponent<Animator>();
            _toolTipAnimator.Play(INACTIVE_ANIMATION);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isActive && other.CompareTag(GameInternalTags.PLAYER))
                _toolTipAnimator.Play(SHOW_ANIMATION);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_isActive && other.CompareTag(GameInternalTags.PLAYER))
                _toolTipAnimator.Play(HIDE_ANIMATION);
        }

        public void ActivateToolTip()
        {
            _toolTipAnimator.Play(ACTIVE_ANIMATION);
            _isActive = true;
        }

        public void DeactivateTooltip()
        {
            _toolTipAnimator.Play(INACTIVE_ANIMATION);
            _isActive = false;
        }

        public void InteractToolTip()
        {
            if (_isActive)
            {
                _isActive = false;
                _toolTipAnimator.Play(INTERACT_ANIMATION);
                _nextTooltip?.ActivateToolTip();
            }
        }
    }
}