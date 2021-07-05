using UI;
using UI.Dialog;
using UI.EndGamePuzzle;
using UI.GameOver;
using UI.Minigame;
using UI.Notification;
using UI.Options;
using UI.PauseMenu;
using UnityEngine;

namespace GameManagers
{
    public class GameHudManager : MonoBehaviour
    {
        public NotificationUI notificationHud;
        public UIDialog uiDialogHud;
        public MinigameUI minigameHud;
        public EndGameUI endGameUI;
        public DamageUI damageUI;
        public UIOptions optionsUI;
        public UIAreYouSure areyouSureUI;
        public LoadingView optionsLoadingUI;
        public UIGameOver gameOverUI;

        public static GameHudManager instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        private void Update()
        {
            if(GameStateManager.currentState != GameState.RUNNING) return;
            uiDialogHud.RunUpdate();
        }
    }
}