using System;
using JetBrains.Annotations;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using Utilities.VariableManagement;

namespace UI.MainMenu
{
    public class SubSlotsScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _chapterTitleText;

        public Action OnReturn;

        private MenuScreenAnimationController _slotScreenAnimationController;
        private MenuScreenAnimationController _subSlotScreenAnimationController;
        private int _slot;

        private void Awake()
        {
            _subSlotScreenAnimationController = GetComponent<MenuScreenAnimationController>();
        }

        public void EnableSubSlotScreen(MenuScreenAnimationController p_slotScreenAnimationController, int p_slot, string p_slotText)
        {
            _chapterTitleText.text = p_slotText;
            _slotScreenAnimationController = p_slotScreenAnimationController;
            _slot = p_slot;

            _slotScreenAnimationController.Hide(delegate ()
            {
                _subSlotScreenAnimationController.Show();
            });
        }


        [UsedImplicitly]
        public void LoadSlot()
        {
            SaveGameManager.instance.LoadSlot(_slot);

            foreach (Button __button in gameObject.GetComponentsInChildren<Button>())
                __button.interactable = false;

            _subSlotScreenAnimationController.Hide(delegate ()
            {
                LoadingView.instance.FadeIn(delegate
                {
                    SceneManager.LoadScene(ScenesConstants.GAME);
                }, VariablesManager.uiVariables.defaultFadeInSpeed * 2f);
            });
        }

        [UsedImplicitly]
        public void DeleteSlot()
        {
            SaveGameManager.instance.DeleteSlot(_slot);

            OnReturn?.Invoke();

            ReturnScreen();
        }

        [UsedImplicitly]
        public void Abort()
        {
            ReturnScreen();
        }

        private void ReturnScreen()
        {
            _subSlotScreenAnimationController.Hide(delegate ()
            {
                _slotScreenAnimationController.Show();
            });
        }
    }
}
