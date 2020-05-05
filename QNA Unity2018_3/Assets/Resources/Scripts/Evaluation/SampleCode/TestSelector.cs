using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAC.Evaluation.Sample
{
    public class TestSelector : MonoBehaviour
    {
        #region VARIABLES
        [SerializeField]
        private Evaluator evaluator;
        [SerializeField]
        private GameObject continueButton;
        [Header("Selection Buttons")]
        [SerializeField]
        private Color selected = Color.blue;
        [SerializeField]
        private Color normal = Color.white;

        [SerializeField]
        private SelectionButton[] Buttons;
        #endregion

        #region PUBLIC METHODS
        public void DisplayTestsIDs()
        {
            UnselectButtons();
            HideAllButtons();
            DisableContinueButton();

            for (int test = 0; test < evaluator.AllTests.Length; ++test)
            {
                string id = evaluator.AllTests[test].TestID;
                Buttons[test].SetID(test, id);
                ShowButton(Buttons[test]);
            }
        }

        public void SelectButton(int buttonIndex)
        {
            UnselectButtons();
            HighlightButton(Buttons[buttonIndex]);
            evaluator.CurrentTestIndex = (ushort)Buttons[buttonIndex].GetTestIndex();
            EnableContinueButton();
        }
        #endregion

        #region PRIVATE METHODS
        private void DisableContinueButton()
        {
            continueButton.SetActive(false);
        }
        private void EnableContinueButton()
        {
            continueButton.SetActive(true);
        }

        private void HideAllButtons()
        {
            foreach (SelectionButton button in Buttons)
                button.gameObject.SetActive(false);
        }
        private void ShowButton(SelectionButton button)
        {
            button.gameObject.SetActive(true);
        }

        private void HighlightButton(SelectionButton button)
        {
            button.SetBackgroundColor(selected);
        }
        private void UnselectButtons()
        {
            foreach (SelectionButton button in Buttons)
                button.SetBackgroundColor(normal);
        }
        #endregion

    }
}