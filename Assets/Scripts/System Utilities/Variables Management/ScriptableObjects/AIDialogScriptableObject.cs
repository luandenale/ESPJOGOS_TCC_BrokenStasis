using System.Collections.Generic;
using Utilities.UI;
using UnityEngine;

namespace Utilities.VariableManagement
{
    [CreateAssetMenu(fileName = "AIDialogs")]
    public class AIDialogScriptableObject : ScriptableObject
    {
        public List<DialogConversationUnit> GameDialogs;
    }
}
