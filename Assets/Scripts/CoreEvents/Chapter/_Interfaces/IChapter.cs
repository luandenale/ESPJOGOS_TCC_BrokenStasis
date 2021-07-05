using System.Collections.Generic;
using CoreEvent.GameEvents;

namespace CoreEvent.Chapters
{
    public interface IChapter
    {
        ChapterTypeEnum chapterType { get; }
        List<IGameEvent> gameEvents { get; }

        void ChapterStart();
        void ChapterEnd();
    }
}
