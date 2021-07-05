using GameManagers;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Utilities;
using Utilities.Audio;
using Utilities.UI;
using Utilities.VariableManagement;
namespace CoreEvent.GameEvents
{
    public class GameEvent_CreditsCutScene : MonoBehaviour, IGameEvent
    {
        [SerializeField] private GameEventTypeEnum _gameEventType;
        [SerializeField] private GameObject _mainCamera;

        private bool _hasRun;
        private VideoPlayer _videoPlayer;

        public GameEventTypeEnum gameEventType { get { return _gameEventType; } }
        public bool hasRun { get { return _hasRun; } }

        private void Awake()
        {
            _hasRun = false;
        }

        public void RunPermanentEvents() { }

        public void RunSingleTimeEvents()
        {
            _hasRun = true;

            _videoPlayer = _mainCamera.GetComponent<VideoPlayer>();
            if (_videoPlayer == null) _videoPlayer = _mainCamera.AddComponent<VideoPlayer>();

            _videoPlayer.playOnAwake = false;
            _videoPlayer.clip = VariablesManager.gameplayVariables.cutsceneCreditsVideo;
            _videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            _videoPlayer.loopPointReached += HandleCutSceneEnd;

            InputController.GamePlay.MouseEnabled = false;
            InputController.GamePlay.InputEnabled = false;

            GameHudManager.instance.uiDialogHud.StartDialog(DialogEnum.ACT_03_CONTROL_PANEL_GREETINGS, delegate ()
            {
                LoadingView.instance.FadeIn(delegate ()
                {
                    AudioManager.instance.Stop(AudioNameEnum.PLAYER_HEARTBEAT);
                    GameHudManager.instance.damageUI.ResetHud();
                    _videoPlayer.SetDirectAudioVolume(0, VariablesManager.gameplayVariables.cutsceneCreditsVideoVolume);
                    _videoPlayer.Play();

                    LoadingView.instance.FadeOut(delegate ()
                    {
                        InputController.GamePlay.InputEnabled = false;
                    }, VariablesManager.uiVariables.defaultFadeOutSpeed * 0.5f);

                }, VariablesManager.uiVariables.defaultFadeInSpeed);
            }
            , false);
        }

        private void HandleCutSceneEnd(UnityEngine.Video.VideoPlayer videoPlayer)
        {
            _videoPlayer.Stop();

            _videoPlayer.loopPointReached -= HandleCutSceneEnd;

            InputController.GamePlay.MouseEnabled = true;
            InputController.GamePlay.InputEnabled = true;
            SceneManager.LoadScene(ScenesConstants.MENU);
        }
    }
}
