using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraShakerController
{
    public static IEnumerator Shake(float p_duration, float p_magnitude, float p_changeFactor)
    {
        float __initialDuration = p_duration;
        p_magnitude = p_magnitude / 10;

        while (p_duration > 0)
        {
            float __x = Random.Range(-1f, 1f) * p_magnitude;
            float __z = Random.Range(-1f, 1f) * p_magnitude;

            if (p_duration < __initialDuration / 2)
            {
                p_magnitude = p_magnitude > 0 ? p_magnitude - p_changeFactor / (p_duration * 100) : 0;
            }

            Camera.main.transform.localPosition = new Vector3(__x, 0, __z);

            p_duration -= Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.localPosition = Vector3.zero;
    }
}
