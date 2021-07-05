using System.Collections.Generic;
using System.Linq;
using CoreEvent.Chapters;
using CoreEvent.GameEvents;
using UI;
using UnityEngine;
using Utilities;
using Utilities.VariableManagement;

namespace GameManagers
{
    public class ChapterManager : MonoBehaviour
    {
        public ChapterTypeEnum initialChapter;
        public static ChapterManager instance;

        private List<IChapter> _chapters = new List<IChapter>();
        private IChapter _currentChapter;
        private int _currentChapterIndex;

        public IChapter currentChapter
        {
            get
            {
                return _currentChapter;
            }
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void InitializeChapters()
        {
            _chapters = gameObject.GetComponentsInChildren<IChapter>().ToList();

            GameEventManager.SetGameEvents(ChapterManager.instance.GetGameEvents());

            foreach (IChapter __chapter in _chapters)
            {
                if (__chapter.chapterType == initialChapter)
                {
                    _currentChapter = __chapter;
                    _currentChapterIndex = _chapters.FindIndex(c => c.Equals(__chapter));

                    break;
                }
            }

            RunPreviousGameEvents();

            _currentChapter.ChapterStart();

            if(_currentChapterIndex > 0)
            {
                InputController.GamePlay.InputEnabled = true;
                InputController.GamePlay.MouseEnabled = true;
                
                LoadingView.instance.FadeOut(null, VariablesManager.uiVariables.defaultFadeOutSpeed * 2f);
            }
        }

        private void RunPreviousGameEvents()
        {
            if (_currentChapterIndex > 0)
            {
                for (int i = 0; i < _currentChapterIndex; i++)
                {
                    foreach (IGameEvent __gameEvent in _chapters[i].gameEvents)
                    {
                        GameEventManager.RunGameEvent(__gameEvent.gameEventType, false);
                    }
                }
            }
        }

        public void GoToNextChapter()
        {
            _currentChapter.ChapterEnd();

            if(_currentChapterIndex + 1 == _chapters.Count)
                return;

            _currentChapterIndex++;

            _currentChapter = _chapters[_currentChapterIndex];

            _currentChapter.ChapterStart();
        }
       
        public List<IGameEvent> GetGameEvents()
        {
            List<IGameEvent> __allGameEvents = new List<IGameEvent>();

            foreach(IChapter _chapter in _chapters)
            {
                __allGameEvents.AddRange(_chapter.gameEvents);
            }

            return __allGameEvents;
        }
    }
}