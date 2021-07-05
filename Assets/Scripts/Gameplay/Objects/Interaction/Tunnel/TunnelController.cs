using System.Collections.Generic;
using UnityEngine;

public class TunnelController : MonoBehaviour
{
    [SerializeField] private GameObject _tunnelLightBlocker;
    [SerializeField] private List<GameObject> _connectionPoints;

    public void EnableLightBlocker()
    {
        // _tunnelLightBlocker.SetActive(true);
    }

    public void DisableLightBlocker()
    {
        // _tunnelLightBlocker.SetActive(false);
    }

    public Vector3 GetSiblingPosition(GameObject _collidedPoint)
    {
        return _connectionPoints.Find(gameObject => gameObject != _collidedPoint).transform.position;
    }
}