using UnityEngine;

namespace Utilities
{
    public class CustomObjectId : MonoBehaviour
    {
        public string uniqueId { get; private set; }

        private void Awake()
        {
            this.uniqueId = System.Guid.NewGuid().ToString();
        }
    }
}