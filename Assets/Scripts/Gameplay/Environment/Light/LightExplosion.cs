using GameManagers;
using Gameplay.Lighting;
using UnityEngine;
using Utilities;
using Utilities.Audio;

public class LightExplosion : TriggerColliderController
{
    [SerializeField] private LightController _light;

    private bool _activated = false;

    private void Awake()
    {
        onTriggerEnter = HandlePlayerEnterTriggerCollider;
    }

    private void HandlePlayerEnterTriggerCollider(Collider other)
    {
        if (!other.CompareTag(GameInternalTags.PLAYER) || _activated) return;
        AudioManager.instance.PlayAtPosition(AudioNameEnum.ENVIRONMENT_LIGHT_EXPLOSION, _light.gameObject.transform.position, false, AudioRange.LOW);
        _light.SetLightState(LightEnum.OFF);
        _activated = true;
    }
}
