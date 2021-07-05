using GameManagers;
using UnityEngine;
using Utilities;
using Utilities.VariableManagement;

namespace Gameplay.Player.Motion
{

    public class PlayerMovement : IFixedUpdateBehaviour
    {
        #region PRIVATE READONLY FIELDS
        private readonly CharacterController _charController;
        private readonly Transform _playerTransform;
        private readonly PlayerTunnelBehaviour _playerTunnelBehaviour;
        #endregion PRIVATE READONLY FIELDS

        #region PRIVATE FIELDS
        private Vector3 _previousPosition;
        #endregion PRIVATE FIELDS

        private bool _movingSideways;
        private bool _movingBackward;
        private bool _running;
        private bool _crouching;
        private float _currentSpeed;

        public PlayerMovement(CharacterController p_charController,
                                Transform p_playerTransform,
                                PlayerTunnelBehaviour p_playerTunnelBehaviour)
        {
            _charController = p_charController;
            _playerTransform = p_playerTransform;
            _playerTunnelBehaviour = p_playerTunnelBehaviour;

            _movingSideways = false;
            _movingBackward = false;
            PlayerStatesManager.onStateChanged += HandleStateChanged;
        }

        public void RunFixedUpdate()
        {
            if (GameStateManager.currentState != GameState.RUNNING || !InputController.GamePlay.InputEnabled) return;

            SetMovementVariables();
            SetMovingState();

            HandleMovement();
            HandleDirection();
            HandleCrouching();

            PlayerStatesManager.onPlayerCrouching(_crouching);
        }

        private void HandleStateChanged(PlayerState p_playerState)
        {
            switch (p_playerState)
            {
                case PlayerState.RUNNING_FORWARD:
                case PlayerState.RUNNING_SIDEWAYS:
                    _currentSpeed = VariablesManager.playerVariables.regularSpeed * VariablesManager.playerVariables.fastSpeedMultiplier;
                    break;
                case PlayerState.WALKING_BACKWARD:
                    _currentSpeed = VariablesManager.playerVariables.regularSpeed * VariablesManager.playerVariables.slowSpeedMultiplier;
                    break;
                default:
                    _currentSpeed = VariablesManager.playerVariables.regularSpeed;
                    break;
            }
        }

        private void HandleMovement()
        {
            Vector3 __moveDirection = InputController.GamePlay.NavigationAxis();
            float __crouchingSpeed = VariablesManager.playerVariables.regularSpeed * VariablesManager.playerVariables.slowSpeedMultiplier;

            if (_crouching)
                _charController.SimpleMove(__moveDirection.normalized * __crouchingSpeed * Time.deltaTime);
            else
                _charController.SimpleMove(__moveDirection.normalized * _currentSpeed * Time.deltaTime);
        }

        private void HandleDirection()
        {
            Vector2 __mouseOnScreen = (Vector2)UnityEngine.Camera.main.ScreenToViewportPoint(InputController.GamePlay.MousePosition());
            LooktoPosition(__mouseOnScreen);
        }

        private void LooktoPosition(Vector2 p_targetPosition)
        {
            Vector2 __positionOnScreen = UnityEngine.Camera.main.WorldToViewportPoint(_playerTransform.position);
            float __angle = Mathf.Atan2(__positionOnScreen.y - p_targetPosition.y, __positionOnScreen.x - p_targetPosition.x) * Mathf.Rad2Deg;

            _playerTransform.rotation = Quaternion.Euler(new Vector3(0f, -__angle, 0f));
        }

        private void HandleCrouching()
        {
            if (_crouching)
            {
                _charController.height = VariablesManager.playerVariables.playerHeightWhenCrouching;
                _charController.center = new Vector3(_charController.center.x, -VariablesManager.playerVariables.playerHeightWhenCrouching / VariablesManager.playerVariables.playerHeightWhenUp, _charController.center.z);
            }
            else
            {
                _charController.height = VariablesManager.playerVariables.playerHeightWhenUp;
                _charController.center = new Vector3(_charController.center.x, 0, _charController.center.z);
            }
        }

        #region  CHECKERS
        private void SetMovingState()
        {
            if (InputController.GamePlay.NavigationAxis() != Vector3.zero)
            {
                if (_running && !_crouching)
                {
                    if (_movingSideways) PlayerStatesManager.SetPlayerState(PlayerState.RUNNING_SIDEWAYS);
                    else if (_movingBackward) PlayerStatesManager.SetPlayerState(PlayerState.WALKING_BACKWARD);
                    else PlayerStatesManager.SetPlayerState(PlayerState.RUNNING_FORWARD);
                }
                else
                {
                    if (_movingSideways) PlayerStatesManager.SetPlayerState(PlayerState.WALKING_SIDEWAYS);
                    else if (_movingBackward) PlayerStatesManager.SetPlayerState(PlayerState.WALKING_BACKWARD);
                    else PlayerStatesManager.SetPlayerState(PlayerState.WALKING_FORWARD);
                }
            }
            else
                PlayerStatesManager.SetPlayerState(PlayerState.STATIC);
        }

        private void SetMovementVariables()
        {
            Vector3 __direction = (_playerTransform.position - _previousPosition).normalized;
            __direction = Quaternion.AngleAxis(-90, Vector3.down) * __direction;
            float __dotProduct = Vector3.Dot(__direction, _playerTransform.forward.normalized);

            if (__dotProduct > 0.5)
            {
                // Debug.Log("forward");
                _movingBackward = false;
                _movingSideways = false;
            }
            else if (__dotProduct < 0)
            {
                // Debug.Log("Backward");
                _movingSideways = false;
                _movingBackward = true;
            }
            else if (__dotProduct != 0)
            {
                // Debug.Log("Side");
                _movingSideways = true;
                _movingBackward = false;
            }

            _running = InputController.GamePlay.Run();
            _crouching = InputController.GamePlay.Crouch();
            _playerTunnelBehaviour.isCrouching = _crouching;

            _previousPosition = _playerTransform.position;
        }
        #endregion CHECKERS
    }
}