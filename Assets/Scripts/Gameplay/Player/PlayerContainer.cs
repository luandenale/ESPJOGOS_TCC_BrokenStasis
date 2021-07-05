using Gameplay.Player.Animation;
using Gameplay.Player.Item;
using Gameplay.Player.Motion;
using UnityEngine;

namespace Gameplay.Player
{
    [System.Serializable]
    public struct PlayerSuitData
    {
        public PlayerSuitEnum suitType;
        public GameObject suitGameObject;
        public Animator suitAnimator;
        public PlayerAnimationEventHandler suitAnimationEventHandler;
    }
    public class PlayerContainer : MonoBehaviour
    {
        [Header("Movement Reference")]
        [Space(5)]
        public CharacterController characterController;
        public Transform playerTransform;
        public PlayerTunnelBehaviour playerTunnelBehaviour;

        [Header("Suits")]
        [Space(5)]
        public PlayerSuitData[] suits;

        [Header("Light References")]
        [Space(5)]
        public Light[] playerLights;
        public GameObject playerIlluminationGameObject;
        public Transform lightDetectableObject;
        public Collider lightDetectableCollider;
        public LayerMask lightDetectorLayersToDetect;
        public Transform lightOriginTransform;

        [Header("Sound Colliders")]
        [Space(5)]
        public Collider lowSoundCollider;
        public GameObject lowSoundShader;
        public Collider mediumSoundCollider;
        public GameObject mediumSoundShader;
        public Collider loudSoundCollider;
        public GameObject loudSoundShader;

        private void Awake()
        {
            if (characterController == null)
                throw new MissingComponentException("CharacterController not found in PlayerContainer!");
            if (playerTransform == null)
                throw new MissingComponentException("PlayerTransform not found in PlayerContainer!");
            if (suits == null)
                throw new MissingComponentException("PlayerSuits not found in PlayerContainer!");
            if (lowSoundCollider == null)
                throw new MissingComponentException("LowSoundCollider not found in PlayerContainer!");
            if (lowSoundShader == null)
                throw new MissingComponentException("LowSoundShader not found in PlayerContainer!");
            if (mediumSoundCollider == null)
                throw new MissingComponentException("MediumSoundCollider not found in PlayerContainer!");
            if (mediumSoundShader == null)
                throw new MissingComponentException("MediumSoundShader not found in PlayerContainer!");
            if (loudSoundCollider == null)
                throw new MissingComponentException("LoudSoundCollider not found in PlayerContainer!");
            if (loudSoundShader == null)
                throw new MissingComponentException("LoudSoundShader not found in PlayerContainer!");
        }
    }
}

