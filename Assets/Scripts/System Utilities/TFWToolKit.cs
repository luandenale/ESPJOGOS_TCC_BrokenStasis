using System;
using System.Collections;
using System.Collections.Generic;
using GameManagers;
using UnityEngine;

namespace Utilities
{
    public static class TFWToolKit
    {
        public static void StartCoroutine(IEnumerator p_coroutine)
        {
            CustomSceneManager.instance.StartCoroutine(p_coroutine);
        }

        public static void StopCoroutine(IEnumerator p_coroutine)
        {
            CustomSceneManager.instance.StopCoroutine(p_coroutine);
        }

        public static void CancelTimer(Coroutine p_timerCoroutine)
        {
            CustomSceneManager.instance.StopCoroutine(p_timerCoroutine);
        }

        public static Coroutine Timer(float p_duration, Action p_callback)
        {
            return CustomSceneManager.instance.StartCoroutine(TimerCoRoutine(p_duration, p_callback));
        }

        public static Coroutine CountdownTimer(float p_duration, Action p_finishCallback, Action<TimeSpan> p_timeSpan = null)
        {
            return CustomSceneManager.instance.StartCoroutine(CountdownTimerCoRoutine(p_duration, p_finishCallback, p_timeSpan));
        }

        private static IEnumerator CountdownTimerCoRoutine(float p_duration, Action p_finishCallback, Action<TimeSpan> p_timeSpan = null)
        {
            float __timeRemaining = p_duration;
            
            while (__timeRemaining >= 0)
            {
                p_timeSpan?.Invoke(TimeSpan.FromSeconds(__timeRemaining));
                
                __timeRemaining -= Time.deltaTime;

                yield return null;
            }

            p_finishCallback?.Invoke();
        }


        private static IEnumerator TimerCoRoutine(float p_duration, Action p_callback)
        {
            yield return new WaitForSeconds(p_duration);

            p_callback?.Invoke();
        }

        public static List<Sprite> GetSpriteShuffledSubList(List<Sprite> p_inputList, int p_sublistSize)
        {
            List<Sprite> __shuffledSubList = new List<Sprite>();

            if (p_sublistSize < p_inputList.Count)
            {
                for (int i = 0; i < p_sublistSize; i++)
                {
                    int __randomIndex = UnityEngine.Random.Range(0, p_inputList.Count - 1);

                    __shuffledSubList.Add(p_inputList[__randomIndex]);

                    p_inputList.RemoveAt(__randomIndex);
                }
            }

            return __shuffledSubList;
        }
    }
}
