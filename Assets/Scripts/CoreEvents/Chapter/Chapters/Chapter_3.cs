using System.Collections.Generic;
using System.Linq;
using CoreEvent.GameEvents;
using GameManagers;
using Gameplay.Objects.Items;
using SaveSystem;
using UI;
using UI.ToolTip;
using UnityEngine;
using UnityEngine.AI;

namespace CoreEvent.Chapters
{
    public class Chapter_3 : MonoBehaviour, IChapter
    {
        [SerializeField] private ChapterTypeEnum _chapterType;
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private CharacterController _playerCharacterController;
        [SerializeField] private ItemKeyCard _itemKeyCard;
        [SerializeField] private NavMeshSurface _navMeshSurface;
        [SerializeField] private GameObject[] _gameObjectsToActivate;
        [SerializeField] private ToolTip[] _activateToolTips;
        [SerializeField] private ToolTip[] _deactivatedToolTips;

        public ChapterTypeEnum chapterType
        {
            get
            {
                return _chapterType;
            }
        }

        public List<IGameEvent> gameEvents
        {
            get
            {
                return gameObject.GetComponentsInChildren<IGameEvent>().ToList();
            }
        }

        public void ChapterStart()
        {
            Debug.Log("STARTED CHAPTER 3");
            AudioManager.instance.StopMusic(1);

            GameHudManager.instance.notificationHud.ShowText("Press [F] to toggle Lantern", 8);

            foreach(ToolTip __tooltip in _activateToolTips)
            {
                __tooltip.ActivateToolTip();
            }

            foreach(ToolTip __tooltip in _deactivatedToolTips)
            {
                __tooltip.DeactivateTooltip();
            }

            _playerCharacterController.enabled = false;
            _playerCharacterController.transform.position = _startPosition;
            _playerCharacterController.enabled = true;

            if (SaveGameManager.instance.currentGameSaveData.chapter != chapterType)
            {
                SaveGameManager.instance.currentGameSaveData = GameplayManager.instance.GetCurrentGameData();

                SaveGameManager.instance.SaveSlot(SaveGameManager.instance.currentGameSaveData.saveSlot);
            }

            foreach(GameObject __gameObject in _gameObjectsToActivate)
            {
                __gameObject.SetActive(true);
            }

            _itemKeyCard.SetEnabled(true);
            _navMeshSurface.BuildNavMesh();
        }

        public void ChapterEnd()
        {
            Debug.Log("FINISHED CHAPTER 3");
        }
    }
}
