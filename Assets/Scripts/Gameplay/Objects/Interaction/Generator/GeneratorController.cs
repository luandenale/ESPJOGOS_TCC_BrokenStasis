
using CoreEvent.GameEvents;
using GameManagers;
using Gameplay.Player.Motion;
using UI.ToolTip;
using UnityEngine;
using Utilities;

namespace Gameplay.Objects.Interaction
{
    public class GeneratorController : InteractionObjectWithColliders
    {
        [SerializeField] private ToolTip _minigameToolTip;

        private bool _enabled;

        private void Awake()
        {
            _enabled = false;
        }

        public void SetEnabled(bool p_enabled)
        {
            _enabled = p_enabled;
        }

        private void Start()
        {
            GameHudManager.instance.minigameHud.onMinigameSuccess = HandleMinigameSuccess;
            GameHudManager.instance.minigameHud.onMinigameFailed = HandleMinigameFailed;
        }

        public override void Interact()
        {
            if(_enabled)
            {   
                PlayerStatesManager.SetPlayerState(PlayerState.PRESS_BUTTON);
                _minigameToolTip.InteractToolTip();
                GameHudManager.instance.minigameHud.ShowMinigame();
            }
        }

        private void HandleMinigameSuccess()
        {
            GameEventManager.RunGameEvent(GameEventTypeEnum.GENERATOR_COMPLETE_MINIGAME);
        }

        private void HandleMinigameFailed()
        {
            InputController.GamePlay.InputEnabled = true;
            InputController.GamePlay.MouseEnabled = true;
        }
    }
}
