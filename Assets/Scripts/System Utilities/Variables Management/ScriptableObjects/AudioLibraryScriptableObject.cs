using System.Collections.Generic;
using UnityEngine;
using Utilities.Audio;

namespace Utilities.VariableManagement
{
 
    // [CreateAssetMenu(fileName = "AudioVariables")]   
    public class AudioLibraryScriptableObject : ScriptableObject
    {
        public List<AudioClipUnit> AudioLibrary;
    }
}