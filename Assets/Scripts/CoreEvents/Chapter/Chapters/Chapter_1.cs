using System.Collections.Generic;
using System.Linq;
using CoreEvent.GameEvents;
using GameManagers;
using Gameplay.Objects.Interaction;
using UI;
using UI.ToolTip;
using UnityEngine;
using Utilities;
using Utilities.Audio;
using Utilities.UI;

namespace CoreEvent.Chapters
{
    public class Chapter_1 : MonoBehaviour, IChapter
    {
        [SerializeField] private ChapterTypeEnum _chapterType;
        [SerializeField] private DoorController _doorController;

        [SerializeField] private ControlsSplash _splashControls;
        [SerializeField] private ControlsSplash _splashHeadphones;
        [SerializeField] private ToolTip _firstDoorToolTip;

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
            LoadingView.instance.InstantBlackScreen();
            AudioManager.instance.PlayMusic(AudioNameEnum.SOUND_TRACK_GAMEPLAY, 5);

            _splashControls.ShowControlsSplash(delegate ()
            {
                _splashHeadphones.ShowControlsSplash(() =>
                {
                    _firstDoorToolTip.ActivateToolTip();

                    LoadingView.instance.FadeOut(delegate ()
                    {
                        InputController.UI.InputEnabled = true;

                        GameHudManager.instance.uiDialogHud.StartDialog(DialogEnum.ACT_01_WAKE_UP_MESSAGE);
                    });

                    _doorController.LockDoor();
                    _doorController.onDoorLocked = delegate ()
                    {
                        GameHudManager.instance.uiDialogHud.StartDialog(DialogEnum.ACT_01_LOCKED_DOOR);
                    };
                });
            });
        }

        public void ChapterEnd()
        {
            Debug.Log("FINISHED CHAPTER 1");
        }
    }
}
