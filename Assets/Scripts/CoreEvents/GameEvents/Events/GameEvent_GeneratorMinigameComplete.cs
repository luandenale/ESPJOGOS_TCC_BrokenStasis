using System.Collections;
using GameManagers;
using Gameplay.Objects.Interaction;
using Gameplay.Objects.Items;
using UnityEngine;
using Utilities.Audio;
using Utilities.UI;

namespace CoreEvent.GameEvents
{
    public class GameEvent_GeneratorMinigameComplete : MonoBehaviour, IGameEvent
    {
        [SerializeField] private GameEventTypeEnum _gameEventType;
        [SerializeField] private GeneratorController _generatorController;
        [SerializeField] private Animator _generatorAnimator;
        [SerializeField] private Transform _generatorEngineTransform;
        [SerializeField] private ItemFlashlightBatteries _itemFlashlightBatteries;
        [SerializeField] private GameObject _eventLightBlink;

        private const string ANIMATION_GENERATOR_TURN_ON = "GeneratorTurnOn";
        private const string ANIMATOR_GENERATOR_SPEED_PARAMETER = "GeneratorRotationSpeed";

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
            _generatorController.SetEnabled(false);
            _itemFlashlightBatteries.SetEnabled(true);
            _eventLightBlink.SetActive(true);
        }

        public void RunSingleTimeEvents()
        {
            RunPermanentEvents();
            GameHudManager.instance.uiDialogHud.StartDialog(DialogEnum.ACT_02_MINIGAME_COMPLETE);

            _generatorAnimator.Play(ANIMATION_GENERATOR_TURN_ON);
            AudioManager.instance.FadeInAtPosition(AudioNameEnum.ENVIRONMENT_GENERATOR_ENGINE, 10, _generatorEngineTransform.position, AudioRange.HIGH, null, true, null);
            // AudioManager.instance.FadeIn(AudioNameEnum.ENVIRONMENT_GENERATOR_ENGINE, 10, null, true);
            StartCoroutine(IncreaseGeneratorRotationSpeed());
            _hasRun = true;
        }

        private IEnumerator IncreaseGeneratorRotationSpeed()
        {
            var __speedMultiplier = 0f;

            while (__speedMultiplier < 10)
            {
                __speedMultiplier += Time.deltaTime;
                _generatorAnimator.SetFloat(ANIMATOR_GENERATOR_SPEED_PARAMETER, __speedMultiplier);
                yield return null;
            }
        }
    }
}
