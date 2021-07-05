using GameManagers;
using UnityEngine;

namespace UI.PauseMenu
{
    public class PauseMenuController : MonoBehaviour
    {
        private Animator _animator;

        private const string PAUSE_GAME_ANIMATION = "PauseGame";
        private const string RESUME_GAME_ANIMATION = "ResumeGame";
        private const string OPTIONS_TITLE_GAME_PAUSED = "Paused";

        private void Awake()
        {
            _animator = GetComponent<Animator>() ?? throw new MissingComponentException("Animator not found!");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameStateManager.currentState == GameState.RUNNING)
                    PauseGame();
                else if (GameStateManager.currentState == GameState.PAUSED)
                    ResumeGame();
            }
        }

        public void PauseGame()
        {
            GameHudManager.instance.optionsUI.StartUIHandlers(OPTIONS_TITLE_GAME_PAUSED, 
                                                                delegate { ResumeGame(); }, 
                                                                delegate { ResumeGame(); }, 
                                                                delegate { ResumeGame(); });
            Time.timeScale = 0;
            AudioManager.instance.PauseAllAudioSources();
            _animator.Play(PAUSE_GAME_ANIMATION);
            GameStateManager.SetGameState(GameState.PAUSED);
        }

        public void ResumeGame()
        {
            GameHudManager.instance.optionsUI.CloseAll();
            Time.timeScale = 1;
            AudioManager.instance.ResumeAllAudioSources();
            _animator.Play(RESUME_GAME_ANIMATION);
            GameStateManager.SetGameState(GameState.RUNNING);
        }
    }
}