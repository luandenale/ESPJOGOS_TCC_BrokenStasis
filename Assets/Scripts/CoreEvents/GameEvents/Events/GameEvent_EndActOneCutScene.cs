using System.Collections.Generic;
using GameManagers;
using Gameplay.Objects.Interaction;
using UI;
using UnityEngine;
using UnityEngine.Video;
using Utilities;
using Utilities.Audio;
using Utilities.VariableManagement;

namespace CoreEvent.GameEvents
{
    public class GameEvent_EndActOneCutScene : TriggerColliderController, IGameEvent
    {
        [SerializeField] private GameEventTypeEnum _gameEventType;
        [SerializeField] private DoorController _doorController;
        [SerializeField] private List<GameObject> _lightsList;
        [SerializeField] private GameObject _mainCamera;
        [SerializeField] private GameObject _enemiesManager;
        [SerializeField] private CharacterController _playerCharacterController;
        [SerializeField] private Animator _corridorC8Animator;
        [SerializeField] private Vector3 _inRoomPosition;

        private VideoPlayer _videoPlayer;
        private bool _hasRun;

        public bool hasRun { get { return _hasRun; } }
        public GameEventTypeEnum gameEventType { get { return _gameEventType; } }

        private void Awake()
        {
            _hasRun = false;
        }

        public void RunPermanentEvents() { }

        public void RunSingleTimeEvents()
        {
            if (_hasRun) return;

            _videoPlayer = _mainCamera.GetComponent<VideoPlayer>();
            if (_videoPlayer == null) _videoPlayer = _mainCamera.AddComponent<VideoPlayer>();

            _videoPlayer.playOnAwake = false;
            _videoPlayer.clip = VariablesManager.gameplayVariables.cutsceneSplinterVideo;
            _videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            _videoPlayer.loopPointReached += HandleCutSceneEnd;
            _videoPlayer.SetDirectAudioVolume(0, VariablesManager.gameplayVariables.cutsceneSplinterVideoVolume);

            InputController.GamePlay.MouseEnabled = false;
            InputController.GamePlay.InputEnabled = false;

            AudioManager.instance.StopMusic(2.0f);

            GameStateManager.SetGameState(GameState.CUTSCENE);

            _videoPlayer.Play();

            foreach (GameObject __light in _lightsList)
            {
                __light.SetActive(true);
            }
            DisableAllEnemies();
        }

        private void HandleCutSceneEnd(UnityEngine.Video.VideoPlayer videoPlayer)
        {
            _doorController.isDoorOpen = false;
            _doorController.SetDoorState();
            _doorController.LockDoor();

            _playerCharacterController.enabled = false;
            _playerCharacterController.transform.position = _inRoomPosition;
            _corridorC8Animator.Play("FadeOut");
            _playerCharacterController.enabled = true;

            _videoPlayer.Stop();
            _videoPlayer.loopPointReached -= HandleCutSceneEnd;
            
            _hasRun = true;
            
            InputController.GamePlay.MouseEnabled = true;
            InputController.GamePlay.InputEnabled = true;
            LoadingView.instance.FadeOut(null);

            GameStateManager.SetGameState(GameState.RUNNING);
        }

        private void DisableAllEnemies()
        {
            var __childrenObjects = _enemiesManager.GetComponentsInChildren<Transform>();
            List<Transform> __enemies = new List<Transform>(__childrenObjects);
            __enemies.RemoveAt(0);

            foreach (Transform __enemy in __enemies)
                __enemy.gameObject.SetActive(false);
        }
    }
}
