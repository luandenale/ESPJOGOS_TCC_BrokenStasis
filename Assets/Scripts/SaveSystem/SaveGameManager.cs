using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SaveSystem
{
    public class SaveGameManager : MonoBehaviour
    {
        public SlotSaveData currentGameSaveData;
        private GameSaveData _saveData = new GameSaveData();

        private const string FILE_PATH = "/Savedata";
        private const string FILE_NAME = "/SaveFile.jua";

        private static SaveGameManager _instance;
        public static SaveGameManager instance
        {
            get
            {
                return _instance ?? (_instance = InstanceInitialize());
            }
        }

        private static SaveGameManager InstanceInitialize()
        {
            _instance = Instantiate(new GameObject("SaveManager")).AddComponent<SaveGameManager>();

            DontDestroyOnLoad(_instance);

            return _instance;
        }

        public void SaveSlot(int p_slot)
        {
            _saveData.saveData[p_slot - 1] = currentGameSaveData;

            string __saveDataJson = JsonUtility.ToJson(_saveData, true);
            byte[] __bytes = System.Text.Encoding.UTF8.GetBytes(__saveDataJson);

            if (!Directory.Exists(Application.dataPath + FILE_PATH))
                Directory.CreateDirectory(Application.dataPath + FILE_PATH);

            File.WriteAllBytes(Application.dataPath + FILE_PATH + FILE_NAME, __bytes);
        }

        public bool HasSaveSlot(int p_slot)
        {
            return _saveData.saveData[p_slot - 1] != null && _saveData.saveData[p_slot - 1].saveSlot != 0;
        }

        public bool SaveFileExists()
        {
            return File.Exists(Application.dataPath + FILE_PATH + FILE_NAME);
        }

        public void NewSlot(int p_slot)
        {
            currentGameSaveData = new SlotSaveData();
            currentGameSaveData.saveSlot = p_slot;

            SaveSlot(p_slot);
        }

        public void LoadSlot(int p_slot)
        {
            byte[] __bytes = File.ReadAllBytes(Application.dataPath + FILE_PATH + FILE_NAME);
            string __saveDataJson = System.Text.Encoding.UTF8.GetString(__bytes);

            _saveData = JsonUtility.FromJson<GameSaveData>(__saveDataJson);

            currentGameSaveData = _saveData.saveData[p_slot - 1];
        }

        public void DeleteSlot(int p_slot)
        {
            currentGameSaveData = new SlotSaveData();

            SaveSlot(p_slot);
        }

        public void Initialize()
        {
            for(int i = 0; i < 3; i++)
            {
                _saveData.saveData[i] = new SlotSaveData();
                
                if(SaveFileExists())
                    LoadSlot(i+1);
            }
        }

#if UNITY_EDITOR
        [MenuItem("TFW Tools/Utilities/Clear Save Data")]
        public static void ClearSaveData()
        {
            EditorUtility.DisplayDialog("Save Game Cleared", "Deleted Save Game at " + Application.dataPath + FILE_PATH + FILE_NAME, "OK");

            if (File.Exists(Application.dataPath + FILE_PATH + FILE_NAME))
                File.Delete(Application.dataPath + FILE_PATH + FILE_NAME);
        }
#endif

    }
}
