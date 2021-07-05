using System;
using UI.ToolTip;
using UnityEngine;
using Utilities;

namespace Gameplay.Player.Motion
{
    public class PlayerTunnelBehaviour : MonoBehaviour
    {
        [SerializeField] private ToolTip _crouchToolTip;
        public bool isCrouching { get; set; }

        private bool _isCrossing;
        private Vector3 _targetPosition;

        private float _crouchingSpeed;
        private Transform _playerTransform;
        private CharacterController _charController;
        private TunnelController _lastStoredTunnelController;

        public void InitializePlayerTunnelBehaviour(float p_crouchingSpeed, Transform p_playerTransform, CharacterController p_charController)
        {
            _crouchingSpeed = p_crouchingSpeed;
            _playerTransform = p_playerTransform;
            _charController = p_charController;
        }

        private void OnTriggerStay(Collider other)
        {      
            if (other.CompareTag(GameInternalTags.TUNNEL) && isCrouching && !_isCrossing)
            {
                _lastStoredTunnelController = other.gameObject.GetComponentInParent<TunnelController>();

                _lastStoredTunnelController.DisableLightBlocker();

                SetMovingInitialState(other.gameObject);

                _isCrossing = true;

                _crouchToolTip.InteractToolTip();
            }
        }

        private void SetMovingInitialState(GameObject p_tunnelGameObject)
        {
            _charController.enabled = false;
            _playerTransform.position = p_tunnelGameObject.transform.position;
            _charController.enabled = true;

            _targetPosition = _lastStoredTunnelController.GetSiblingPosition(p_tunnelGameObject);

            InputController.GamePlay.InputEnabled = false;

            LooktoPosition(_targetPosition);

            PlayerStatesManager.SetPlayerState(PlayerState.WALKING_FORWARD);
        }

        private void Update()
        {
            if (_isCrossing)
            {
                MoveTowardsTarget(_targetPosition);
            }
        }

        private void MoveTowardsTarget(Vector3 p_target)
        {
            Vector3 __offset = p_target - _playerTransform.position;

            if (__offset.magnitude > 0.5f)
            {
                __offset = __offset.normalized * _crouchingSpeed * 0.025f;
                _charController.Move(__offset * Time.deltaTime);
            }
            else
            {
                PlayerStatesManager.SetPlayerState(PlayerState.STATIC);
                InputController.GamePlay.InputEnabled = true;
                _isCrossing = false;
                _lastStoredTunnelController.EnableLightBlocker();
            }
        }

        private void LooktoPosition(Vector2 p_targetPosition)
        {
            Vector2 __positionOnScreen = UnityEngine.Camera.main.WorldToViewportPoint(_playerTransform.position);
            float __angle = Mathf.Atan2(__positionOnScreen.y - p_targetPosition.y, __positionOnScreen.x - p_targetPosition.x) * Mathf.Rad2Deg;

            _playerTransform.rotation = Quaternion.Euler(new Vector3(0f, -__angle, 0f));
        }
    }
}