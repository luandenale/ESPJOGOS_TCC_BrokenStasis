using System;
using System.Collections.Generic;
using CoreEvent.Chapters;
using Gameplay.Player.Item;
using SaveSystem.Player;

namespace SaveSystem
{
    [Serializable]
    public class SlotSaveData
    {
        public int saveSlot = 0;
        public PlayerSaveData playerData;
        public List<DoorSaveData> doorsList = new List<DoorSaveData>();
        public ChapterTypeEnum chapter = ChapterTypeEnum.CHAPTER_1;
        public List<ItemEnum> inventoryList = new List<ItemEnum>();
    }
}
