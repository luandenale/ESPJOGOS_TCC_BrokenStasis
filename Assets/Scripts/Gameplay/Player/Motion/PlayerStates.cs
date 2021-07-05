using System;

namespace Gameplay.Player.Motion
{
    public enum PlayerState
    {
        STATIC,
        WALKING_FORWARD,
        WALKING_SIDEWAYS,
        WALKING_BACKWARD,
        RUNNING_FORWARD,
        RUNNING_SIDEWAYS,
        DEAD,
        HIT,
        PRESS_BUTTON,
        PICK_ITEM_ON_GROUND,
        PICK_ITEM,
        INTERACT_WITH_ENDLEVEL_DOOR,
        EXITED_ENDLEVEL_DOOR_AREA
    }

    public static class PlayerStatesManager
    {
        public static PlayerState currentState { get; private set; }
        public static Action<PlayerState> onStateChanged;
        public static Action<bool> onPlayerCrouching;

        public static void SetPlayerState(PlayerState p_newState)
        {
            if (currentState != p_newState)
            {
                currentState = p_newState;

                if (onStateChanged != null) onStateChanged(currentState);
            }
        }
    }
}
