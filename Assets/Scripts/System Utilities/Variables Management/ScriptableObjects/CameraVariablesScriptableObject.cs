using UnityEngine;

namespace Utilities.VariableManagement
{
    // [CreateAssetMenu(fileName = "CameraVariables")]
    public class CameraVariablesScriptableObject : ScriptableObject
    {
        public float cameraDistanceFromPlayer;
        public int renderedLightsInCamera;
    }   
}
