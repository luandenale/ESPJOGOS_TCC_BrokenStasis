using System;
using TMPro;
using UI.PauseMenu;
using UnityEngine;
using Utilities;
using Utilities.VariableManagement;

namespace UI.Options.Video
{
    public class VideoConfirmationPopUp : UIAreYouSure
    {
        [SerializeField] private TextMeshProUGUI _countdownText;

        private Coroutine _countdownTimer;

        public override void StartUIHandlers(Action p_handleYesSelection, Action p_handleNoSelection = null)
        {
            _countdownText.text = TimeSpan.FromSeconds(VariablesManager.uiVariables.videoConfirmationPopUpDuration).ToString(@"ss");
            _countdownTimer = null;

            base.StartUIHandlers(p_handleYesSelection, p_handleNoSelection);

            _countdownTimer = TFWToolKit.CountdownTimer(VariablesManager.uiVariables.videoConfirmationPopUpDuration, delegate()
            {
                p_handleNoSelection?.Invoke();
                Close();
            }, delegate(TimeSpan p_timeSpan)
            {
                _countdownText.text = p_timeSpan.ToString(@"ss");
            });
        }

        public override void Close()
        {
            base.Close();
            TFWToolKit.CancelTimer(_countdownTimer);
        }
    }
}
