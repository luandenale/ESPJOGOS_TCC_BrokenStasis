using GameManagers;
using Gameplay.Objects.Interaction;
using Gameplay.Player.Item;
using UnityEngine;

namespace CoreEvent.GameEvents
{
    public class GameEvent_ChangeSuit : MonoBehaviour, IGameEvent
    {
        [SerializeField] private GameEventTypeEnum _gameEventType;
        [SerializeField] private GameObject _suitModel;
        [SerializeField] private DoorController _doorController;
        [SerializeField] private GeneratorController _generatorController;

        private SuitChangeController _suitChangeController;

        public GameEventTypeEnum gameEventType
        {
            get
            {
                return _gameEventType;
            }
        }

        private bool _hasRun;
        public bool hasRun
        {
            get
            {
                return _hasRun;
            }
        }

        private void Awake()
        {
            _suitChangeController = new SuitChangeController();
            _hasRun = false;
        }

        public void RunPermanentEvents()
        {
            _suitModel.SetActive(false);

            GameplayManager.instance.onPlayerSuitChange(PlayerSuitEnum.SUIT1);

            _doorController.UnlockDoorLock();
            _generatorController.SetEnabled(true);
        }

        public void RunSingleTimeEvents()
        {
            _suitChangeController.ChangeSuit(PlayerSuitEnum.SUIT1, delegate ()
            {
                RunPermanentEvents();
                ChapterManager.instance.GoToNextChapter();
                _hasRun = true;
            });
        }
    }
}
