using System.Collections;
using UnityEngine;
using Utilities;

namespace Gameplay.GameplayEvent
{
    public class FootstepsEnemy : MonoBehaviour
    {
        [SerializeField] private TriggerColliderController _activateCollider;
        [SerializeField] private TriggerColliderController _deactivateCollider;
        [SerializeField] private Animator _footstepsAnimator;
        [SerializeField] private GameObject _fotstepsGameObject;
        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private string _animationToPlay;

        private bool _alreadyActivated = false;

        private void Start()
        {
            _activateCollider.onTriggerEnter = HandleActivate;
            _deactivateCollider.onTriggerEnter = HandleDeactivate;
        }

        private void HandleActivate(Collider p_collider)
        {
            if (p_collider.tag.Equals(GameInternalTags.PLAYER) && !_alreadyActivated)
            {
                _alreadyActivated = true;
                _footstepsAnimator.Play(_animationToPlay);
            }
        }

        private void HandleDeactivate(Collider p_collider)
        {
            if (p_collider.tag.Equals(GameInternalTags.PLAYER) && _alreadyActivated && !_footstepsAnimator.GetCurrentAnimatorStateInfo(0).IsName("END"))
            {
                _footstepsAnimator.enabled = false;

                foreach (SpriteRenderer __sprite in _fotstepsGameObject.GetComponentsInChildren<SpriteRenderer>())
                {
                    if (__sprite.color.a != 0)
                    {
                        StartCoroutine(FadeOutSprite(__sprite));
                    }
                }
            }
        }

        private IEnumerator FadeOutSprite(SpriteRenderer p_spriteRenderer)
        {
            float __elapsedTime = 0;
            float __startValue = p_spriteRenderer.color.a;
            while (__elapsedTime < _fadeOutDuration)
            {
                __elapsedTime += Time.deltaTime;

                float __newAlpha = Mathf.Lerp(__startValue, 0f, __elapsedTime / _fadeOutDuration);
                p_spriteRenderer.color = new Color(p_spriteRenderer.color.r, p_spriteRenderer.color.g, p_spriteRenderer.color.b, __newAlpha);

                yield return null;
            }
        }
    }
}