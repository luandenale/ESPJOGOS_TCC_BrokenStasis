using CoreEvent.Chapters;
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
    public class SlotsScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _slotsTexts;
        [SerializeField] private SubSlotsScreen _subSlotScreen;

        private MenuScreenAnimationController _slotsScreenAnimationController;

        private void Start()
        {
            SetSlotsTexts();

            _slotsScreenAnimationController = GetComponent<MenuScreenAnimationController>();
        }

        [UsedImplicitly]
        public void LoadSlot1()
        {
            LoadSlot(1);
        }

        [UsedImplicitly]
        public void LoadSlot2()
        {
            LoadSlot(2);
        }

        [UsedImplicitly]
        public void LoadSlot3()
        {
            LoadSlot(3);
        }

        private void LoadSlot(int p_slot)
        {

            if(SaveGameManager.instance.HasSaveSlot(p_slot))
            {
                _subSlotScreen.OnReturn = SetSlotsTexts;

                _subSlotScreen.EnableSubSlotScreen(_slotsScreenAnimationController, p_slot, GetMissionText(p_slot));
            }
            else
            {
                foreach(Button __button in gameObject.GetComponentsInChildren<Button>())
                    __button.interactable = false;

                SaveGameManager.instance.NewSlot(p_slot);

                LoadingView.instance.FadeIn(delegate ()
                {
                    SceneManager.LoadScene(ScenesConstants.GAME);
                }, VariablesManager.uiVariables.defaultFadeInSpeed * 2f);
            }
        }

        private void SetSlotsTexts()
        {
            for(int i = 0; i < 3; i++)
            {
                if(SaveGameManager.instance.HasSaveSlot(i+1))
                    _slotsTexts[i].text = GetMissionText(i+1);
                else
                    _slotsTexts[i].text = "NEW FILE";
            }
        }

        private string GetMissionText(int p_slot)
        {
            SaveGameManager.instance.LoadSlot(p_slot);

            switch(SaveGameManager.instance.currentGameSaveData.chapter)
            {
                case ChapterTypeEnum.CHAPTER_1:
                    return "CHAPTER [1] THE AWAKENING";
                case ChapterTypeEnum.CHAPTER_2:
                    return "CHAPTER [2] SEARCHING FOR ANSWERS";
                case ChapterTypeEnum.CHAPTER_3:
                    return "CHAPTER [3] FIRST CONTACT";
                default:
                    return "UNKNOWN CHAPTER";
            }
        }
    }
}
