using System;

namespace SaveSystem
{
    [Serializable]
    public class GameSaveData
    {
        public SlotSaveData[] saveData = new SlotSaveData[3];
    }
}
