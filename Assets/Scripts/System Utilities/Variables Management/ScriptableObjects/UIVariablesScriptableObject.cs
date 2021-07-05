using UnityEngine;

namespace Utilities.VariableManagement
{
    // [CreateAssetMenu(fileName = "UIVariables")]
    public class UIVariablesScriptableObject : ScriptableObject
    {
        public float defaultNotificationDuration;
        public int defaultReadingWPM;
        public float defaultFadeInSpeed;
        public float defaultFadeOutSpeed;
        public float generatorMinigameDuration;
        public float controlsSplashDuration;
        public float videoConfirmationPopUpDuration;
    }
}