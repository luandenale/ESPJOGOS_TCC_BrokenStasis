using System;
namespace GameManagers
{
    public enum GameState
    {
        RUNNING,
        PAUSED,
        GAMEOVER,
        CUTSCENE
    }

    public static class GameStateManager
    {
        public static GameState currentState { get; private set; }

        public static Action<GameState> onStateChanged;

        public static void SetGameState(GameState p_newState)
        {
            if (currentState != p_newState)
            {
                currentState = p_newState;

                if (onStateChanged != null) onStateChanged(currentState);
            }
        }
    }
}