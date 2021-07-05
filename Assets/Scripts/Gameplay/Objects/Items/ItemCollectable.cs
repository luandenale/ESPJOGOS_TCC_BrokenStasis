using UnityEngine;
using GameManagers;
using Utilities.Audio;
using Gameplay.Objects.Interaction;

namespace Gameplay.Objects.Items
{
    public class ItemCollectable : InteractionObjectWithColliders
    {
        [SerializeField] ItemHeightEnum itemHeight;

        public override void Interact()
        {
            AudioManager.instance.Play(AudioNameEnum.ITEM_PICKUP);
            if (itemHeight.Equals(ItemHeightEnum.GROUND))
            {
                Debug.Log("Picked collectable item on ground.");
            }
            else if (itemHeight.Equals(ItemHeightEnum.HIGH))
            {
                Debug.Log("Picked collectable item on high place.");
            }
        }
    }
}
