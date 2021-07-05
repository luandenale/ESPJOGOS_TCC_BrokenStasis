using UnityEngine;

namespace Gameplay.Lighting
{
    [RequireComponent(typeof(Light))]
    [RequireComponent(typeof(Animator))]
    public class LightController : MonoBehaviour
    {
        private Animator _animator;
        private Light _light;

        [SerializeField] private LightEnum _lightState;

        public LightEnum lightState { get { return _lightState; } }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _light = GetComponent<Light>();

            SetLightState(_lightState);
        }

        public void SetLightState(LightEnum p_lightState)
        {
            _animator.Play(p_lightState.ToString());
            _lightState = p_lightState;
        }
    }
}
