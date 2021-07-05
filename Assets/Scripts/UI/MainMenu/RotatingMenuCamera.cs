using UnityEngine;

namespace UI.MainMenu
{
    public class RotatingMenuCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 _movingVector;

        private void Update()
        {
            transform.Rotate(_movingVector);
        }
    }
}
