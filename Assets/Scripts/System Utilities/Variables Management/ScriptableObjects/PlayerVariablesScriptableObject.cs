using UnityEngine;

namespace Utilities.VariableManagement
{
    // [CreateAssetMenu(fileName = "PlayerVariables")]
    public class PlayerVariablesScriptableObject : ScriptableObject
    {
        public int maxHealth;
        public float regularSpeed;
        public float slowSpeedMultiplier;
        public float fastSpeedMultiplier;
        public float playerHeightWhenUp;
        public float playerHeightWhenCrouching;
        public bool isPlayerInvencible;
    }   
}
