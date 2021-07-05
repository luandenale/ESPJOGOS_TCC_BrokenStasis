using GameManagers;
using UnityEngine;
using Utilities;

namespace Gameplay.Player.Sensors
{
    public class PlayerVision : MonoBehaviour
    {
        [SerializeField] private Transform _headPosition;
        [SerializeField] private LayerMask _layersToDetect;

        private void Awake()
        {
            if (_headPosition == null)
                throw new MissingComponentException("HeadPosition not found in PlayerVision!");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameInternalTags.ENEMY) && HasDirectViewOfHiddenObject(other.gameObject))
            {
                // foreach (SkinnedMeshRenderer __meshRenderer in __meshRenderers)
                //     __meshRenderer.enabled = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(GameInternalTags.ENEMY))
            {
                if (HasDirectViewOfHiddenObject(other.gameObject))
                {
                    // foreach (SkinnedMeshRenderer __meshRenderer in __meshRenderers)
                    //     __meshRenderer.enabled = true;
                    VFXManager.instance.FadeOutMaterial(other.GetComponentInParent<CustomObjectId>().uniqueId);
                }
                else
                {
                    // foreach (SkinnedMeshRenderer meshRenderer in __meshRenderers)
                    //     meshRenderer.enabled = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(GameInternalTags.ENEMY))
            {
                var __meshRenderers = other.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

                // foreach (SkinnedMeshRenderer meshRenderer in __meshRenderers)
                //         meshRenderer.enabled = false;
            }
        }

        private bool HasDirectViewOfHiddenObject(GameObject p_hiddenGameObject)
        {
            RaycastHit __hit;
            Vector3 __toPosition = p_hiddenGameObject.transform.position;
            Vector3 __direction = __toPosition - _headPosition.position;

            if (Physics.Raycast(_headPosition.position, __direction, out __hit, 100f, _layersToDetect))
            {
                if (__hit.collider.CompareTag(GameInternalTags.ENEMY))
                    Debug.DrawRay(_headPosition.position, __direction, Color.magenta);

                return __hit.collider.CompareTag(GameInternalTags.ENEMY);
            }

            return false;
        }
    }
}
