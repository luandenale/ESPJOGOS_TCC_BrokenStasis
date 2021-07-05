using UnityEngine;
using Utilities;

namespace Gameplay.Objects.Interaction
{
    public abstract class InteractionObjectWithColliders : MonoBehaviour, IInteractionObject
    {
        protected bool _isActive = false;

        public virtual void Interact() { }

        public virtual void RunFixedUpdate() { }

        public virtual void RunUpdate()
        {
            if (_isActive && InputController.GamePlay.Interact())
            {
                Interact();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("PlayerInteractor"))
                _isActive = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("PlayerInteractor"))
                _isActive = false;
        }
    }
}
