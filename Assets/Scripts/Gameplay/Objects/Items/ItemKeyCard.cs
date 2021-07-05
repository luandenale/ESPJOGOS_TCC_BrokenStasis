
using GameManagers;
using Gameplay.Objects.Interaction;
using Gameplay.Player.Item;
using Gameplay.Player.Motion;
using UI.ToolTip;
using UnityEngine;

namespace Gameplay.Objects.Items
{
    public class ItemKeyCard : InteractionObjectWithColliders
    {
        [SerializeField] private GameObject _modelGameObject;
        [SerializeField] private ToolTip _keyCardToolTip;
        
        private bool _enabled;

        private void Awake()
        {
            _enabled = false;
            _modelGameObject.SetActive(_enabled);
        }

        public void SetEnabled(bool p_enabled)
        {
            _enabled = p_enabled;
            _modelGameObject.SetActive(_enabled);
        }

        public override void Interact()
        {
            if(_enabled)
            {
                _keyCardToolTip.InteractToolTip();
                PlayerStatesManager.SetPlayerState(PlayerState.PICK_ITEM_ON_GROUND);
                GameplayManager.instance.onPlayerCollectedItem(ItemEnum.KEYCARD);

                GameHudManager.instance.notificationHud.ShowText("Collected KeyCard");

                SetEnabled(false);
            }
        }
    }
}