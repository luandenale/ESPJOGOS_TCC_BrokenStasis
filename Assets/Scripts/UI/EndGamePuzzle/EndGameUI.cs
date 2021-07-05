using System.Collections;
using GameManagers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EndGamePuzzle
{
    public class EndGameUI : MonoBehaviour
    {
        [SerializeField] private Animator _hudAnimator;
        [SerializeField] private Image[] _loadingBars;

        [SerializeField] private UIAnimationEventHandler _endGameEventHandler;

        private const string SHOW_ENDGAME_HUD_ANIMATION = "Show";
        private const string HIDE_ENDGAME_HUD_ANIMATION = "Hide";

        // TODO: Extract this consts to variables
        private const float RESET_DURATION = 1f;
        private const float RESET_PERCENTAGE_SPLIT = 100f;

        private void Awake()
        {
            _endGameEventHandler.OnHideAnimationEnd = HandleHideAnimationEnd;
            _endGameEventHandler.OnShowAnimationEnd = HandleShowAnimationEnd;

            foreach(Image __loadingBar in _loadingBars)
                __loadingBar.fillAmount = 0f;
        }
        
        public void ShowUI()
        {
            _hudAnimator.Play(SHOW_ENDGAME_HUD_ANIMATION);
        }

        public void HideUI()
        {
            _hudAnimator.Play(HIDE_ENDGAME_HUD_ANIMATION);
        }

        public void UpdateBar(int p_barIndex, float p_fillAmmount)
        {
            _loadingBars[p_barIndex].fillAmount = p_fillAmmount;
        }

        public void ResetBar(int p_barIndex)
        {
            CustomSceneManager.instance.StartCoroutine(ResetCoRoutine(p_barIndex));
        }

        private IEnumerator ResetCoRoutine(int p_barIndex)
        {
            float __secondsPerIteration = RESET_DURATION / RESET_PERCENTAGE_SPLIT;
            float __percentagePerIteration = 1 / RESET_PERCENTAGE_SPLIT;
            
            while(_loadingBars[p_barIndex].fillAmount > 0)
            {
                _loadingBars[p_barIndex].fillAmount -= __percentagePerIteration;

                yield return new WaitForSeconds(__secondsPerIteration);
            }

            HideUI();

            yield return null;
        }

        private void HandleHideAnimationEnd()
        {
            
        }

        private void HandleShowAnimationEnd()
        {
            
        }
    }
}