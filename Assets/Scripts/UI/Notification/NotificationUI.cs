using System.Collections.Generic;
using GameManagers;
using TMPro;
using UnityEngine;
using Utilities;
using Utilities.Audio;
using Utilities.VariableManagement;

namespace UI.Notification
{
    public class NotificationUI : MonoBehaviour, IUIAutoHideText
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private TextMeshProUGUI _notificationText;
        [SerializeField] private UIAnimationEventHandler _notificationEventHandler;

        private const string SHOW_NOTIFICATION_ANIMATION = "Show";
        private const string HIDE_NOTIFICATION_ANIMATION = "Hide";

        private Queue<KeyValuePair<string, float>> _notificationQueue = new Queue<KeyValuePair<string, float>>();
        private bool _isVisible = false;
        private float _defaultNotificationDuration;

        private void Awake()
        {
            _defaultNotificationDuration = VariablesManager.uiVariables.defaultNotificationDuration;

            _notificationEventHandler.OnHideAnimationEnd = delegate ()
            {
                _isVisible = false;
                DisplayNextNotification();
            };
        }

        private void DisplayNextNotification()
        {
            if (_notificationQueue.Count > 0)
            {
                KeyValuePair<string, float> __nextNotifcation = _notificationQueue.Dequeue();
                DisplayNotification(__nextNotifcation.Key, __nextNotifcation.Value);
            }
        }

        public void ShowText(string p_text, float p_duration = 0)
        {
            if (_isVisible || _notificationQueue.Count > 0)
            {
                _notificationQueue.Enqueue(new KeyValuePair<string, float>(p_text, p_duration));
                return;
            }
            DisplayNotification(p_text, p_duration);
        }

        private void DisplayNotification(string p_text, float p_duration)
        {
            _isVisible = true;
            _notificationText.text = p_text;

            _animator.Play(SHOW_NOTIFICATION_ANIMATION);

            AudioManager.instance.Play(AudioNameEnum.ITEM_PICKUP, false);
            
            _notificationEventHandler.OnShowAnimationEnd = delegate ()
            {
                float __duration = p_duration;
                
                if(p_duration < _defaultNotificationDuration)
                    __duration = _defaultNotificationDuration;
                else if (p_duration == _defaultNotificationDuration)
                    __duration = CalculateAutoHideTime(p_text);

                TFWToolKit.Timer(__duration, delegate ()
                {
                    _animator.Play(HIDE_NOTIFICATION_ANIMATION);
                });
            };
        }

        private float CalculateAutoHideTime(string p_text)
        {
            int __wordCount = p_text.Split(' ').Length;
            float __readingSpeed = (60f / VariablesManager.uiVariables.defaultReadingWPM) * __wordCount;

            if (__readingSpeed < _defaultNotificationDuration)
                return _defaultNotificationDuration;

            return __readingSpeed;
        }

    }
}
