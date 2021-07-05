using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Minigame
{
    public class DamageUI : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _playerLifeDamageSprites;
        [SerializeField] private Animator _animatorComponent;
        [SerializeField] private Image _currentImageComponent;

        private const string GET_HURT_HUD_ANIMATION = "GetHurt";
        private const string RESET_HUD_ANIMATION = "ResetHud";

        private void Start()
        {
            _playerLifeDamageSprites.Reverse();
        }

        public void ReceiveHit(int _currentPlayerLife)
        {
            _currentImageComponent.sprite = _playerLifeDamageSprites[_currentPlayerLife - 1];
            _animatorComponent.Play(GET_HURT_HUD_ANIMATION);
        }

        public void ResetHud()
        {
            if (_currentImageComponent.sprite != null)
                _animatorComponent.Play(RESET_HUD_ANIMATION);
            _currentImageComponent.sprite = null;
        }
    }
}