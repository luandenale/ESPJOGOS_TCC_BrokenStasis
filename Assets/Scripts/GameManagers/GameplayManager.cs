using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Gameplay.Camera;
using Gameplay.Enemy;
using Gameplay.Objects.Interaction;
using Gameplay.Player;
using Gameplay.Player.Item;
using Gameplay.Player.Motion;
using SaveSystem;
using UI;
using UnityEngine;

namespace GameManagers
{
    public class GameplayManager : MonoBehaviour
    {
        // TODO: Extract to Broadcast/Listener system
        #region GAME_EVENTS
        public Action<PlayerSuitEnum> onPlayerSuitChange;
        public Action<ItemEnum> onPlayerCollectedItem;
        public Action<int> onPlayerDamaged;
        #endregion

        [SerializeField] private PlayerContainer _playerContainer;
        [SerializeField] private CameraContainer _cameraContainer;
        [SerializeField] private GameObject _audioListenerGameObject;
        [SerializeField] private GameObject _levelGameObjects;
        [SerializeField] private GameObject _enemiesGameObjects;

        private PlayerBase _player;
        private InventoryController _inventoryController;
        public InventoryController inventoryController { get { return _inventoryController; } }
        private CameraFollowPlayer _cameraFollowPlayer;
        private AudioListenerController _audioListenerController;
        private LevelObjectManager _levelObjectManager;
        private EnemiesManager _enemiesManager;
        private LightPriorityManager _lightPriorityManager;

        private int _saveSlot;

        public static GameplayManager instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;

            RegisterObjectsGraph(_playerContainer);

            onPlayerDamaged += _player.onPlayerDamaged;
            _enemiesManager?.InitializeEnemies();
        }

        private void Start()
        {
            // TODO: Transferir lógica de load e inicialização para classe superior
            StartScenario();

            if (_inventoryController == null)
                RegisterInventoryController(new List<ItemEnum>());

            ChapterManager.instance?.InitializeChapters();
        }

        private void RegisterObjectsGraph(PlayerContainer p_playercontainer)
        {
            RegisterPlayerGraph(p_playercontainer);

            if (_levelGameObjects != null) _levelObjectManager = new LevelObjectManager(_levelGameObjects.GetComponentsInChildren<IInteractionObject>().ToList());
            if (_enemiesGameObjects != null) _enemiesManager = new EnemiesManager(_enemiesGameObjects.GetComponentsInChildren<IEnemy>().ToList());
        }

        private void RegisterPlayerGraph(PlayerContainer p_playercontainer)
        {
            PlayerStatesManager.onPlayerCrouching = null;
            PlayerStatesManager.onStateChanged = null;

            if (p_playercontainer != null)
            {
                _player = new PlayerBase(p_playercontainer);

                if (_cameraContainer != null)
                    _cameraFollowPlayer = new CameraFollowPlayer(p_playercontainer.playerTransform, _cameraContainer.cameraTransform);

                if (_audioListenerGameObject != null)
                    _audioListenerController = new AudioListenerController(p_playercontainer.playerTransform, _audioListenerGameObject.transform);
            }

            _player?.InitializePlayer();

            onPlayerSuitChange = _player?.onSuitChange;
        }

        private void FixedUpdate()
        {
            _player?.RunFixedUpdate();
            _cameraFollowPlayer?.RunFixedUpdate();
            _audioListenerController?.RunFixedUpdate();
            _levelObjectManager?.RunFixedUpdate();
            // NOTE: Not using lightpriorityManager for the moment
            // _lightPriorityManager?.RunFixedUpdate();
        }

        private void Update()
        {
            _levelObjectManager?.RunUpdate();
            _enemiesManager?.RunUpdate();
            _player?.RunUpdate();
        }

        private void RegisterInventoryController(List<ItemEnum> p_list)
        {
            _inventoryController = new InventoryController(p_list);
            onPlayerCollectedItem = _inventoryController?.onPlayerCollectedItem;
        }

        //TODO: Transferir para classe adequada (não é papel do GameplayManager)
        private void StartScenario()
        {
            if (SaveGameManager.instance.currentGameSaveData == null)
            {
                SaveGameManager.instance.Initialize();

                if (SaveGameManager.instance.currentGameSaveData == null || SaveGameManager.instance.currentGameSaveData.saveSlot == 0)
                    SaveGameManager.instance.NewSlot(3);
            }
            _saveSlot = SaveGameManager.instance.currentGameSaveData.saveSlot;

            LoadGame();
        }

        private void LoadGame()
        {
            ChapterManager.instance.initialChapter = SaveGameManager.instance.currentGameSaveData.chapter;

            RegisterInventoryController(SaveGameManager.instance.currentGameSaveData.inventoryList);

            if (SaveGameManager.instance.currentGameSaveData.playerData.health != 0)
                _player.SetPlayerSaveData(SaveGameManager.instance.currentGameSaveData);

            if (SaveGameManager.instance.currentGameSaveData.doorsList != null)
                RestoreDoorsStateFromSaveFile();
        }

        //TODO: Transferir para classe adequada (não é papel do GameplayManager)
        public SlotSaveData GetCurrentGameData()
        {
            SlotSaveData __gameSaveData = _player.GetPlayerSaveData();

            __gameSaveData.chapter = ChapterManager.instance.currentChapter.chapterType;

            __gameSaveData.inventoryList = _inventoryController.inventoryList;

            var __ingameDoorsList = _levelGameObjects.GetComponentsInChildren<DoorController>().ToList();
            __gameSaveData.doorsList = new List<DoorSaveData>();
            foreach (DoorController door in __ingameDoorsList)
            {
                DoorSaveData doorState = new DoorSaveData();
                doorState.parentName = door.transform.parent.name;
                doorState.isDoorOpen = door.isDoorOpen;
                doorState.isDoorLocked = door.isLocked;
                __gameSaveData.doorsList.Add(doorState);
            }

            __gameSaveData.saveSlot = _saveSlot;

            return __gameSaveData;
        }

        private void RestoreDoorsStateFromSaveFile()
        {
            List<DoorController> __doors = _levelGameObjects.GetComponentsInChildren<DoorController>().ToList();
            foreach (DoorSaveData __savedDoorState in SaveGameManager.instance.currentGameSaveData.doorsList)
            {
                foreach (DoorController __ingameDoor in __doors)
                {
                    if (__ingameDoor.transform.parent.name == __savedDoorState.parentName)
                    {
                        __ingameDoor.isDoorOpen = __savedDoorState.isDoorOpen;

                        if (__savedDoorState.isDoorLocked)
                            __ingameDoor.LockDoor();
                        else
                            __ingameDoor.UnlockDoorLock();

                        __ingameDoor.SetDoorState();
                    }
                }
            }
        }
    }
}
