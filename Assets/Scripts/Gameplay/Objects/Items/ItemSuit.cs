using CoreEvent.GameEvents;
using GameManagers;
using Gameplay.Objects.Interaction;
using Gameplay.Player.Motion;
using UI.ToolTip;
using UnityEngine;

namespace Gameplay.Objects.Items
{
    public class ItemSuit : InteractionObjectWithColliders
    {
        private bool _activated;

        [SerializeField] private ToolTip _suitToolTip;

        private void Awake()
        {
            _activated = false;
        }

        public override void Interact()
        {
            if (!_activated)
            {
                _suitToolTip.InteractToolTip();
                
                PlayerStatesManager.SetPlayerState(PlayerState.PICK_ITEM_ON_GROUND);

                GameEventManager.RunGameEvent(GameEventTypeEnum.DRESS_PLAYER);

                _activated = true;
            }
        }
    }
}
