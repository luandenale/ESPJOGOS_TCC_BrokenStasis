using UnityEngine;
using Utilities;

namespace Gameplay
{
    public class AudioListenerController : IFixedUpdateBehaviour
    {

        private readonly Transform _playerTransform;
        private readonly Transform _audioListenerTransform;

        public AudioListenerController(Transform p_playerTransform, Transform p_audioListenerTransform)
        {
            _playerTransform = p_playerTransform;
            _audioListenerTransform = p_audioListenerTransform;
        }

        public void RunFixedUpdate()
        {
            if (InputController.GamePlay.InputEnabled)
            {
                _audioListenerTransform.position = new Vector3(_playerTransform.position.x, _audioListenerTransform.position.y, _playerTransform.position.z);
            }
        }
    }
}
