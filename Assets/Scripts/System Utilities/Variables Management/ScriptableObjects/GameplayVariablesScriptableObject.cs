using UnityEngine;
using UnityEngine.Video;

namespace Utilities.VariableManagement
{
    // [CreateAssetMenu(fileName = "GameplayVariables")]
    public class GameplayVariablesScriptableObject : ScriptableObject
    {
        public VideoClip cutsceneSplinterVideo;
        public float cutsceneSplinterVideoVolume;
        public VideoClip cutsceneCreditsVideo;
        public float cutsceneCreditsVideoVolume;
        public float finalDoorUnlockTimePerStage;
    }   
}