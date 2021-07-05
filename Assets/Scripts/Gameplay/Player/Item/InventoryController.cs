using System;
using System.Collections.Generic;

namespace Gameplay.Player.Item
{
    public class InventoryController
    {
        public Action<ItemEnum> onPlayerCollectedItem;

        private List<ItemEnum> _inventoryList;
        public List<ItemEnum> inventoryList { get { return _inventoryList; } }

        public InventoryController(List<ItemEnum> p_inventoryList)
        {
            _inventoryList = p_inventoryList;
            
            onPlayerCollectedItem = HandleItemCollected;
        }

        private void HandleItemCollected(ItemEnum p_item)
        {
            _inventoryList.Add(p_item);
        }
    }
}