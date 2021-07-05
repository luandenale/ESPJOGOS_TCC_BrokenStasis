using UnityEditor;
using UnityEditor.ShortcutManagement;

[InitializeOnLoadAttribute]
public class DisableEditorShortcutsOnPlay
{
    private const string _playModeProfile = "PlayModeProfile";

    static DisableEditorShortcutsOnPlay ()
    {
        // EditorApplication.playModeStateChanged += DetectPlayModeState;
    }

    private static void DetectPlayModeState (PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            ShortcutManager.instance.activeProfileId = _playModeProfile;
        } 
        else if (state == PlayModeStateChange.ExitingPlayMode)
        {
            ShortcutManager.instance.activeProfileId = ShortcutManager.defaultProfileId;
        }
    }
}