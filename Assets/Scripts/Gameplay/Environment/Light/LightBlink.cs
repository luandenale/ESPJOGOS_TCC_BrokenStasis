using System.Collections.Generic;
using GameManagers;
using Gameplay.Lighting;
using UnityEngine;
using Utilities;
using Utilities.Audio;

public class LightBlink : TriggerColliderController
{
    [SerializeField] private List<LightController> _lights;

    private bool _activated = false;

    private void Awake()
    {
        onTriggerEnter = HandlePlayerEnterTriggerCollider;
    }

    private void HandlePlayerEnterTriggerCollider(Collider other)
    {
        if (!other.CompareTag(GameInternalTags.PLAYER) || _activated) return;
        AudioManager.instance.PlayAtPosition(AudioNameEnum.ENVIRONMENT_LIGHT_BLINKING, this.gameObject.transform.position, false, AudioRange.LOW);
        
        foreach (LightController light in _lights)
        {
            light.SetLightState(LightEnum.REGULAR_FLICKING);
        }
        _activated = true;
    }
}
