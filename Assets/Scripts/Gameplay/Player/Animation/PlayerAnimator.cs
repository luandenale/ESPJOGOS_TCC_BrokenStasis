using GameManagers;
using Gameplay.Player.Motion;
using UnityEngine;
using Utilities.Audio;

namespace Gameplay.Player.Animation
{
    public class PlayerAnimator
    {
        private const string STATIC = "Static";
        private const string WALKING = "Walking";
        private const string WALKING_BACKWARD = "Walking_Backward";
        private const string RUNNING = "Running";
        private const string DEAD = "Dead";
        private const string HIT = "Hit";
        private const string PRESS_BUTTON = "PressButton";
        private const string PICK_ITEM = "PickItem";
        private const string PICK_ITEM_ON_GROUND = "PickItemOnGround";
        private const string INTERACT_WITH_ENDLEVEL_DOOR = "EndLevelDoorInteraction";
        private const string CROUCHING = "Crouching";

        private bool _lastCrouchingState;

        private readonly Animator _animator;
        private readonly PlayerAnimationEventHandler _animationEventHandler;

        public PlayerAnimator(Animator p_animator,
                                PlayerAnimationEventHandler p_animationEventHandler)
        {
            _animator = p_animator;
            _animationEventHandler = p_animationEventHandler;

            PlayerStatesManager.onStateChanged += HandleStateChanged;
            PlayerStatesManager.onPlayerCrouching += HandlePlayerCrouching;

            _animationEventHandler.OnStep += delegate ()
            {
                AudioManager.instance.Play(AudioNameEnum.PLAYER_STEP);
            };
            _animationEventHandler.OnCutsceneEnd += delegate ()
            {
                GameStateManager.SetGameState(GameState.RUNNING);
            };
        }

        private void HandlePlayerCrouching(bool p_crouching)
        {
            if (_lastCrouchingState != p_crouching)
            {
                _lastCrouchingState = p_crouching;
                _animator.SetBool(CROUCHING, p_crouching);
            }
        }

        private void HandleStateChanged(PlayerState p_playerState)
        {
            if (_animator == null || !_animator.isActiveAndEnabled) return;

            ResetAllVariables();

            switch (p_playerState)
            {
                case PlayerState.STATIC:
                    _animator.SetBool(STATIC, true);
                    break;
                case PlayerState.WALKING_FORWARD:
                case PlayerState.WALKING_SIDEWAYS:
                    _animator.SetBool(WALKING, true);
                    break;
                case PlayerState.WALKING_BACKWARD:
                    _animator.SetBool(WALKING_BACKWARD, true);
                    break;
                case PlayerState.RUNNING_FORWARD:
                case PlayerState.RUNNING_SIDEWAYS:
                    _animator.SetBool(RUNNING, true);
                    break;
                case PlayerState.DEAD:
                    _animator.SetTrigger(DEAD);
                    _animator.SetBool(INTERACT_WITH_ENDLEVEL_DOOR, false);
                    break;
                case PlayerState.HIT:
                    _animator.SetTrigger(HIT);
                    _animator.SetBool(INTERACT_WITH_ENDLEVEL_DOOR, false);
                    break;
                case PlayerState.PRESS_BUTTON:
                    _animator.SetTrigger(PRESS_BUTTON);
                    break;
                case PlayerState.PICK_ITEM:
                    _animator.SetTrigger(PICK_ITEM);
                    break;
                case PlayerState.PICK_ITEM_ON_GROUND:
                    _animator.SetTrigger(PICK_ITEM_ON_GROUND);
                    break;
                case PlayerState.INTERACT_WITH_ENDLEVEL_DOOR:
                    _animator.SetBool(INTERACT_WITH_ENDLEVEL_DOOR, true);
                    break;
                case PlayerState.EXITED_ENDLEVEL_DOOR_AREA:
                    _animator.SetBool(INTERACT_WITH_ENDLEVEL_DOOR, false);
                    break;
            }
        }

        private void ResetAllVariables()
        {
            _animator.SetBool(STATIC, false);
            _animator.SetBool(WALKING, false);
            _animator.SetBool(WALKING_BACKWARD, false);
            _animator.SetBool(RUNNING, false);
        }
    }
}