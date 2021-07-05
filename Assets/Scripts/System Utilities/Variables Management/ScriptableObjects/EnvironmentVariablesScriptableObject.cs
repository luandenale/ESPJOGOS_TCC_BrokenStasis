using UnityEngine;

namespace Utilities.VariableManagement
{
    // [CreateAssetMenu(fileName = "EnvironmentVariables")]
    public class EnvironmentVariablesScriptableObject : ScriptableObject
    {
        public float doorSpeed;
        public float doorMaxDelayBeforeOpening;
    }   
}
