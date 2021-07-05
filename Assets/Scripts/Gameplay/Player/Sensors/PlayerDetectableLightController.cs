using UnityEngine;
using Utilities;

namespace Gameplay.Player.Sensors
{
    public class PlayerDetectableLightController : IUpdateBehaviour
    {
        private readonly Transform _lightDetectableObject;
        private readonly Collider _lightDetectableCollider;
        private readonly LayerMask _layersToDetect;
        private readonly Transform _lightOriginTransform;

        public PlayerDetectableLightController(
            Transform p_lightDetectableObject,
            Collider p_lightDetectableCollider,
            LayerMask p_layersToDetect,
            Transform p_lightOriginTransform)
        {
            _lightDetectableObject = p_lightDetectableObject;
            _lightDetectableCollider = p_lightDetectableCollider;
            _layersToDetect = p_layersToDetect;
            _lightOriginTransform = p_lightOriginTransform;
        }

        private bool HasDirectViewOfLightOrigin()
        {
            RaycastHit __hit;
            Vector3 __fromPosition = _lightDetectableObject.transform.position;
            Vector3 __toPosition = _lightOriginTransform.transform.position;
            Vector3 __direction = __toPosition - __fromPosition;

            if (Physics.Raycast(__fromPosition, __direction, out __hit, 50f, _layersToDetect))
            {
                if (__hit.collider.CompareTag(GameInternalTags.PLAYER))
                {
                    Debug.DrawRay(__fromPosition, __direction, Color.green);
                    return true;
                }
            }
            return false;
        }

        public void RunUpdate()
        {
            if (HasDirectViewOfLightOrigin())
                _lightDetectableCollider.enabled = true;
            else
                _lightDetectableCollider.enabled = false;
        }
    }
}
