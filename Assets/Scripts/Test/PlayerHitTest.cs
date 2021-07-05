using GameManagers;
using UnityEngine;
using Utilities;

public class PlayerHitTest : TriggerColliderController
{
    private void Awake()
    {
        onTriggerEnter = HandlePlayerOnTriggerEnter;
        onTriggerExit = HandlePlayerOnTriggerExit;
    }

    private void HandlePlayerOnTriggerExit(Collider obj)
    {
        if (!obj.CompareTag(GameInternalTags.PLAYER)) return;
    }
    
    private void HandlePlayerOnTriggerEnter(Collider obj)
    {
        if (!obj.CompareTag(GameInternalTags.PLAYER)) return;
        GameplayManager.instance.onPlayerDamaged?.Invoke(1);
    }
}
