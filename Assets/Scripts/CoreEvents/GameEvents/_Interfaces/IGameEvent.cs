
namespace CoreEvent.GameEvents
{
    public interface IGameEvent
    {
        GameEventTypeEnum gameEventType { get; }
        bool hasRun { get; }

        void RunSingleTimeEvents();
        void RunPermanentEvents();
    }
}
