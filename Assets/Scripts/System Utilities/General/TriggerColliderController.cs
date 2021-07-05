using System;
using UnityEngine;

namespace Utilities
{
    public class TriggerColliderController : MonoBehaviour
    {
        public Action<Collider> onTriggerEnter;
        public Action<Collider> onTriggerExit;
        public Action<Collider> onTriggerStay;

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExit?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            onTriggerStay?.Invoke(other);
        }
    }
}