
using CoreEvent.GameEvents;
using GameManagers;
using UI.ToolTip;
using UnityEngine;

namespace Gameplay.Objects.Interaction
{
    public class ControlPanelController : InteractionObjectWithColliders
    {
        [SerializeField] private ToolTip _interactionToolTip;

        public override void Interact()
        {
            _interactionToolTip.InteractToolTip();
            
            GameEventManager.RunGameEvent(GameEventTypeEnum.CUTSCENE_CREDITS);
        }
    }
}
