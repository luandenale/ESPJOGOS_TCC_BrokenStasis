using System;
using System.Collections;
using System.Collections.Generic;
using GameManagers;
using UnityEngine;
using Utilities;
using Utilities.Audio;

public class DuctOpenerController : TriggerColliderController
{

    [SerializeField] private GameObject _ductCover;
    [SerializeField] private GameObject _ductCoverFallen;
    [SerializeField] private Collider _newExitPoint;

    private bool _activated = false;

    private void Awake()
    {
        onTriggerEnter = HandlePlayerEnterArea;
    }

    private void HandlePlayerEnterArea(Collider other)
    {
        if (!other.CompareTag(GameInternalTags.PLAYER)) return;
        if (!_activated)
        {
            AudioManager.instance.Play(AudioNameEnum.ENVIRONMENT_DUCT_COVER_FALL);
            _ductCoverFallen.SetActive(true);
            _newExitPoint.enabled = true;
            _ductCover.SetActive(false);
            _activated = true;
        }
    }
}
