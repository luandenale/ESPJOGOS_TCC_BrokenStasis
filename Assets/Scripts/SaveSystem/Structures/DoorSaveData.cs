using System;

namespace SaveSystem
{
    [Serializable]
    public struct DoorSaveData
    {
        public String parentName;
        public bool isDoorOpen;
        public bool isDoorLocked;
    }
}
