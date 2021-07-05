using GameManagers;
using UnityEngine;
using Utilities.Audio;

public class FootstepsEnemySoundController : MonoBehaviour
{
    public void PlayLeftStepSound()
    {
        AudioManager.instance.PlayAtPosition(AudioNameEnum.INVISIBLE_FOOTSTEP_STEP_LEFT, this.gameObject.transform.position, false, AudioRange.LOW);
    }

    public void PlayRightStepSound()
    {
        AudioManager.instance.PlayAtPosition(AudioNameEnum.INVISIBLE_FOOTSTEP_STEP_RIGHT, this.gameObject.transform.position, false, AudioRange.LOW);
    }
}
