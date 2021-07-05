using System;
using UnityEngine;
using Utilities;
using Utilities.VariableManagement;

namespace UI
{
    public class ControlsSplash : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void ShowControlsSplash(Action p_onFinish)
        {
            _animator.Play("Show");

            TFWToolKit.Timer(VariablesManager.uiVariables.controlsSplashDuration, delegate()
            {
                _animator.Play("Hide");

                TFWToolKit.Timer(1f, p_onFinish);
            });
        }
    }
}
