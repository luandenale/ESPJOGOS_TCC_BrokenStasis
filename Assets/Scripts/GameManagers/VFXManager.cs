using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagers
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _soundWaveSmallPrefab;
        [SerializeField] private ParticleSystem _soundWaveMediumPrefab;
        [SerializeField] private ParticleSystem _soundWaveBigPrefab;

        private List<VfxObject> _poolVfxObjects = new List<VfxObject>();

        public static VFXManager instance;

        private const string ANIMATION_VFX_FADE_OUT = "FadeOut";

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        public void CreateNewSoundWave(string p_ownerName, Vector3 p_position, AudioRange p_audioRange, bool p_loop)
        {
            ParticleSystem __instantiated = null;

            switch (p_audioRange)
            {
                case AudioRange.LOW:
                    __instantiated = Instantiate(_soundWaveSmallPrefab, p_position, _soundWaveSmallPrefab.transform.rotation);
                    break;
                case AudioRange.MEDIUM:
                    __instantiated = Instantiate(_soundWaveMediumPrefab, p_position, _soundWaveMediumPrefab.transform.rotation);
                    break;
                case AudioRange.HIGH:
                    __instantiated = Instantiate(_soundWaveBigPrefab, p_position, _soundWaveMediumPrefab.transform.rotation);
                    break;
            }

            var __ps = __instantiated.main;

            __ps.loop = p_loop;

            var currentPosition = __instantiated.transform.position;
            __instantiated.transform.SetPositionAndRotation(new Vector3(currentPosition.x, currentPosition.y + 2, currentPosition.z), __instantiated.transform.rotation);
            __instantiated.gameObject.transform.parent = this.gameObject.transform;

            var __vfxObject = GetVfxObjectFromPool();
            __vfxObject.SetValues(__instantiated.gameObject, __instantiated, p_ownerName);
        }

        public void FadeOutMaterial(string p_ownerName)
        {
            _poolVfxObjects.RemoveAll(x => x == null);
            foreach (VfxObject __vfxObject in _poolVfxObjects)
            {
                if (__vfxObject.particleSystem != null && __vfxObject.particleSystem.isPlaying && __vfxObject.vfxOwnerName.Equals(p_ownerName))
                {
                    StartCoroutine(FadeOutMaterial(__vfxObject.instantiatedVfxObject, __vfxObject.particleSystem));
                }
            }
        }

        private VfxObject GetVfxObjectFromPool()
        {
            _poolVfxObjects.RemoveAll(x => x == null);
            foreach (VfxObject __vfxObject in _poolVfxObjects)
            {
                if (__vfxObject.particleSystem != null && !__vfxObject.particleSystem.isPlaying)
                    return __vfxObject;
            }

            var __newVfxObject = new VfxObject();
            _poolVfxObjects.Add(__newVfxObject);

            return __newVfxObject;
        }

        IEnumerator FadeOutMaterial(GameObject p_gameObject, ParticleSystem p_particleSystem)
        {
            p_particleSystem.Pause();

            if (p_gameObject != null)
            {
                var __shader = p_gameObject.GetComponent<Renderer>();
                var __myColor = new Color(1, 1, 1, 1);

                while (p_gameObject != null && __myColor.a > 0)
                {
                    __shader.material.SetColor("_TintColor", __myColor);
                    __myColor.a -= 0.07f;
                    yield return new WaitForSecondsRealtime(0.01f);
                }
            }
            Destroy(p_gameObject);
        }

        public void SpeedUpVfx(string p_ownerName)
        {
            _poolVfxObjects.RemoveAll(x => x == null);
            foreach (VfxObject __vfxObject in _poolVfxObjects)
            {
                if (__vfxObject.particleSystem != null && __vfxObject.particleSystem.isPlaying && __vfxObject.vfxOwnerName.Equals(p_ownerName))
                {
                    var __ps = __vfxObject.particleSystem.main;
                    __ps.simulationSpeed += 0.15f;
                }
            }
        }
    }

    public class VfxObject
    {
        public GameObject instantiatedVfxObject;
        public ParticleSystem particleSystem;
        public string vfxOwnerName;

        public VfxObject() { }

        public void SetValues(GameObject p_instantiatedVfxObject, ParticleSystem p_particleSystem, string p_vfxOwnerName)
        {
            this.instantiatedVfxObject = p_instantiatedVfxObject;
            this.particleSystem = p_particleSystem;
            this.vfxOwnerName = p_vfxOwnerName;
        }
    }
}
