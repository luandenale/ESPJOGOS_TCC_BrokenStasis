using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UI
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private const float DEFAULT_ANIMATOR_SPEED = 1;
        private static Action _onFadeIn;
        private static Action _onFadeOut;
        private static LoadingView _instance;
        public static LoadingView instance
        {
            get
            {
                return _instance ?? (_instance = InstanceInitialize());
            }
        }

        private static LoadingView InstanceInitialize()
        {
            GameObject _loadingViewGameObject = Resources.Load<GameObject>("LoadingView");

            _instance = Instantiate(_loadingViewGameObject).GetComponent<LoadingView>();

            DontDestroyOnLoad(_instance);

            return _instance;
        }

        public void InstantBlackScreen()
        {
            _animator.Play("InstantBlackScreen");
        }

        public void ClearedScreen()
        {
            _animator.Play("ClearedScreen");
        }

        public void FadeIn(Action p_onFinish, float p_speed = 1)
        {
            _animator.speed = p_speed;
            _onFadeIn = p_onFinish;

            _animator.Play("BlackFadeIn");
        }

        public void FadeOut(Action p_onFinish, float p_speed = 1)
        {
            _animator.speed = p_speed;
            _onFadeOut = p_onFinish;

            _animator.Play("BlackFadeOut");
        }

        [UsedImplicitly]
        private void FinishedFadeInAnimation()
        {
            _onFadeIn?.Invoke();

            _onFadeIn = null;
            _animator.speed = DEFAULT_ANIMATOR_SPEED;
        }

        [UsedImplicitly]
        private void FinishedFadeOutAnimation()
        {
            _onFadeOut?.Invoke();

            _onFadeOut = null;
            _animator.speed = DEFAULT_ANIMATOR_SPEED;
        }
    }
}
