using UnityEngine;

namespace Utilities.UI
{
    [System.Serializable]
    public class DialogTextUnit
    {
        public SpeakerEnum speaker;
        
        [TextArea]
        public string text;
    }
}
