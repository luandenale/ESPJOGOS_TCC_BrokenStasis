using Utilities;

namespace Gameplay.Objects.Interaction
{
    public interface IInteractionObject : IUpdateBehaviour, IFixedUpdateBehaviour
    {
        void Interact();
    }
}
