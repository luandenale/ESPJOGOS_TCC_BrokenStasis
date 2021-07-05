using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.Audio;

namespace GameManagers
{
    public class CustomSceneManager : MonoBehaviour
    {
        public static CustomSceneManager instance;
        private GameObject _lastUISelected;

        private void Awake()
        {
            if (instance == null)
                instance = this;

            _lastUISelected = new GameObject();
        }

        private void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(_lastUISelected);
            }
            else
            {
                _lastUISelected = EventSystem.current.currentSelectedGameObject;
            }
        }
    }
}
