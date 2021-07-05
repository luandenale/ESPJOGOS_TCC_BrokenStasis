using System.Collections;
using System.Collections.Generic;
using GameManagers;
using Gameplay.Lighting;
using Gameplay.Objects.Interaction;
using Gameplay.Objects.Items;
using Gameplay.Scenario;
using UnityEngine;
using Utilities;
using Utilities.Audio;
using Utilities.UI;

namespace CoreEvent.GameEvents
{
    public class GameEvent_GeneratorExplosion : MonoBehaviour, IGameEvent
    {
        [SerializeField] private GameEventTypeEnum _gameEventType;
        [SerializeField] private GameObject _roomGenerator;
        [SerializeField] private GameObject _generatorInterface;
        [SerializeField] private GameObject _roomGeneratorExploded;
        [SerializeField] private ItemFlashlightBatteries _itemFlashlightBatteries;
        [SerializeField] private List<GameObject> _objectsToActivate;
        [SerializeField] private List<GameObject> _objectsToDeactivate;
        [SerializeField] private List<GameObject> _allLights;
        [SerializeField] private List<CoverController> _scenarioCovers;
        [SerializeField] private GameObject _dressingRoomLights;
        [SerializeField] private GameObject _milestone_1_doors;
        [SerializeField] private GameObject _milestone_2_doors;
        [SerializeField] private GameObject _environmentLightExplosion;
        [SerializeField] private GameObject _eventLightBlink;
        [SerializeField] private BoxCollider _room7Collider;

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
            _hasRun = false;
        }

        public void RunPermanentEvents()
        {
            _environmentLightExplosion.SetActive(false);
            _eventLightBlink.SetActive(false);
            _itemFlashlightBatteries.SetCollected(true);
            ChangeGeneratorRoom();
            SwitchObjects();
            TurnOffAllLights();
            DisableCovers();
            OpenDoors();
            _room7Collider.enabled = true;
        }

        public void RunSingleTimeEvents()
        {
            InputController.GamePlay.InputEnabled = false;
            InputController.GamePlay.MouseEnabled = false;
            BlinkLights();

            AudioManager.instance.Play(AudioNameEnum.GENERATOR_ELETRIC_OVERCHARGE, false, delegate ()
            {
                TFWToolKit.StartCoroutine(CameraShakerController.Shake(3, 5f, 1));
                
                TurnOffAllLights();
                var __audioSource = AudioManager.instance.Play(AudioNameEnum.GENERATOR_EXPLOSION, false, delegate ()
                {
                    AudioManager.instance.Stop(AudioNameEnum.ENVIRONMENT_GENERATOR_ENGINE);
                    AudioManager.instance.StopMusic();
                    AudioManager.instance.Stop(AudioNameEnum.ENVIRONMENT_LIGHT_BLINKING);
                    GameHudManager.instance.uiDialogHud.StartDialog(DialogEnum.ACT_03_NO_POWER_WARNING, delegate ()
                    {
                        _hasRun = true;
                        RunPermanentEvents();
                        
                        InputController.GamePlay.InputEnabled = true;
                        InputController.GamePlay.MouseEnabled = true;
                        
                        // Start Chapter 3
                        ChapterManager.instance.GoToNextChapter();
                    });
                });
                StartCoroutine(TurnOffAllLights(__audioSource.clip.length / 1.5f));
            });
        }

        private void ChangeGeneratorRoom()
        {
            _roomGenerator.SetActive(false);
            _generatorInterface.SetActive(false);
            _roomGeneratorExploded.SetActive(true);
        }

        private void SwitchObjects()
        {
            foreach (GameObject gameObject in _objectsToDeactivate)
                gameObject.SetActive(false);
            foreach (GameObject gameObject in _objectsToActivate)
                gameObject.SetActive(true);
        }

        private void BlinkLights()
        {
            foreach (LightController __light in _dressingRoomLights.GetComponentsInChildren<LightController>())
            {
                __light.SetLightState(LightEnum.LOW_ILUMINATION_FLICKING);
            }
        }

        private void TurnOffAllLights()
        {
            foreach (GameObject __light in _allLights)
                __light.SetActive(false);
        }

        private IEnumerator TurnOffAllLights(float p_secondsToWait)
        {
            yield return new WaitForSecondsRealtime(p_secondsToWait);
            TurnOffAllLights();
        }

        private void DisableCovers()
        {
            foreach (CoverController __cover in _scenarioCovers)
                __cover.DisableCover();
        }

        private void OpenDoors()
        {
            List<DoorController> __doorsToOpenAndLock = new List<DoorController>();
            __doorsToOpenAndLock.AddRange(_milestone_1_doors.GetComponentsInChildren<DoorController>());
            __doorsToOpenAndLock.AddRange(_milestone_2_doors.GetComponentsInChildren<DoorController>());
            foreach (DoorController __door in __doorsToOpenAndLock)
            {
                __door.isDoorOpen = true;
                __door.SetDoorState();
                __door.LockDoor();
            }
        }
    }
}
