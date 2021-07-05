using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PauseMenu
{
    public class UIAreYouSure : MonoBehaviour
    {
        public bool isOpen { get; private set; }

        [SerializeField] private Button _buttonYes;
        [SerializeField] private Button _buttonNo;

        private Animator _animator;

        private const string ANIMATION_SHOW_PANEL = "Show";
        private const string ANIMATION_HIDE_PANEL = "Hide";

        private void Start()
        {
            _animator = GetComponent<Animator>() ?? throw new MissingComponentException("Animator not found!");
            if (!_buttonYes) throw new MissingFieldException("Button Yes not assigned");
            if (!_buttonNo) throw new MissingFieldException("Button No not assigned");
        }

        public virtual void StartUIHandlers(Action p_handleYesSelection, Action p_handleNoSelection = null)
        {
            _buttonYes.onClick.RemoveAllListeners();
            _buttonNo.onClick.RemoveAllListeners();

            _buttonYes.onClick.AddListener(delegate
            {
                p_handleYesSelection.Invoke();
                Close();
            });

            _buttonNo.onClick.AddListener(delegate
            {
                p_handleNoSelection?.Invoke();
                Close();
            });

            _animator.Play(ANIMATION_SHOW_PANEL);
            isOpen = true;
        }

        public virtual void Close()
        {
            _animator.Play(ANIMATION_HIDE_PANEL);
            isOpen = false;
        }
    }
}
