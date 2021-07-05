using System;
using GameManagers;
using UnityEngine;
using Utilities;

namespace Gameplay.Enemy.Sensors
{
    public class SensorVision : MonoBehaviour
    {
        [SerializeField] private Transform _eyesTransform;
        [SerializeField] private LayerMask _layersToDetect;

        public Action<Transform> onPlayerDetected;
        public Action<Transform> onPlayerRemainsDetected;
        public Action<Transform> onPlayerLeftDetection;

        public Action<Transform> onLightDetected;
        public Action<Transform> onLightRemainsDetected;
        public Action<Transform> onLightLeftDetection;

        private void Awake()
        {
            if (_eyesTransform == null)
                throw new MissingComponentException("EyesTransform not found in SensorVision!");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameInternalTags.PLAYER) && HasDirectViewOfObject(other.gameObject, GameInternalTags.PLAYER))
                onPlayerDetected?.Invoke(other.transform);
            else if (other.CompareTag(GameInternalTags.DETECTABLE_LIGHT) && HasDirectViewOfObject(other.gameObject, GameInternalTags.DETECTABLE_LIGHT))
                onLightDetected?.Invoke(other.GetComponentsInParent<Transform>()[other.GetComponentsInParent<Transform>().Length - 1]);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(GameInternalTags.PLAYER) && HasDirectViewOfObject(other.gameObject, GameInternalTags.PLAYER))
                onPlayerRemainsDetected?.Invoke(other.transform);
            else if (other.CompareTag(GameInternalTags.DETECTABLE_LIGHT) && HasDirectViewOfObject(other.gameObject, GameInternalTags.DETECTABLE_LIGHT))
                onLightRemainsDetected?.Invoke(other.GetComponentsInParent<Transform>()[other.GetComponentsInParent<Transform>().Length - 1]);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(GameInternalTags.PLAYER))
                onPlayerLeftDetection?.Invoke(other.transform);
            else if (other.CompareTag(GameInternalTags.DETECTABLE_LIGHT) && HasDirectViewOfObject(other.gameObject, GameInternalTags.DETECTABLE_LIGHT))
                onLightLeftDetection?.Invoke(other.GetComponentsInParent<Transform>()[other.GetComponentsInParent<Transform>().Length - 1]);
        }

        private bool HasDirectViewOfObject(GameObject p_detectedObject, string p_objectTag)
        {
            RaycastHit __hit;
            Vector3 __fromPosition = _eyesTransform.position;
            Vector3 __toPosition = p_detectedObject.transform.position;
            Vector3 __direction = __toPosition - __fromPosition;

            if (Physics.Raycast(__fromPosition, __direction, out __hit, 50f, _layersToDetect))
            {
                Debug.DrawRay(__fromPosition, __direction, Color.red);
                return __hit.collider.CompareTag(p_objectTag);
            }
            return false;
        }
    }
}
