using Gameplay.Player.Motion;
using UnityEngine;

namespace Gameplay.Objects.Interaction
{
    public class ButtonController : InteractionObjectWithColliders
    {
        [SerializeField] private GameObject _interactionGameObject;
        private IInteractionObject _interactionObject;

        private void Awake()
        {
            if (_interactionGameObject == null)
                throw new MissingComponentException("Interaction object not found!");
            _interactionObject = _interactionGameObject.GetComponent<IInteractionObject>();
        }

        public override void Interact()
        {
            PlayerStatesManager.SetPlayerState(PlayerState.PRESS_BUTTON);
            _interactionObject.Interact();
        }
    }
}
