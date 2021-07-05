using GameManagers;
using UnityEngine;
using Utilities.Audio;

namespace UI.CustomButtonController
{
    public class CustomButtonController : MonoBehaviour
    {
        public void HandleButtonHighlight()
        {
            if(GameStateManager.currentState == GameState.CUTSCENE) return;
            AudioManager.instance.Play(AudioNameEnum.UI_BUTTON_HIGHLIGHTED);
        }

        public void HandleButtonSelected()
        {
            if(GameStateManager.currentState == GameState.CUTSCENE) return;
            AudioManager.instance.Play(AudioNameEnum.UI_BUTTON_HIGHLIGHTED);
        }

        public void HandleButtonPressed()
        {
            if(GameStateManager.currentState == GameState.CUTSCENE) return;
            AudioManager.instance.Play(AudioNameEnum.UI_BUTTON_PRESSED);
        }

        public void HandleStartButtonPressed()
        {
            if(GameStateManager.currentState == GameState.CUTSCENE) return;
            AudioManager.instance.Play(AudioNameEnum.UI_START_BUTTON_PRESSED);
        }
    }
}
