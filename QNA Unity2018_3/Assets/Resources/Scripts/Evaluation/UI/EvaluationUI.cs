using UnityEngine.UI;
using UnityEngine;
using AGAC.Evaluation.Base;
using AGAC.StandarizedTime;

namespace AGAC.Evaluation
{
    [System.Serializable]
    public class EvaluationUI
    {
        #region VARIABLES
        //Main Display
        [SerializeField] private Text QuestionLabelText;
        [SerializeField] private AnswerButton[] AnswerButtons;
        [SerializeField] private Text CurrentTime;

        //Final Display
        [SerializeField] private Text TotalTime;
        [SerializeField] private Text CorrectAnswers;
        #endregion

        #region PUBLIC METHODS
        public void UpdateResults(StandardTime time,  int correct_answers)
        {
            CorrectAnswers.text = correct_answers.ToString();
            TotalTime.text = time.ToString();
        }

        public void UpdateTme(StandardTime time)
        {
            CurrentTime.text = time.ToString();
        }

        internal void ClearButtonsText() 
        {
            foreach (AnswerButton button in AnswerButtons)
                if(button!=null)
                button.SetAnswer(string.Empty, AnswerType.NONE);
        }

        internal void SetAnswerButtonsColors(Color background, Color text) 
        {
            foreach (AnswerButton button in AnswerButtons) 
            {
                if (button != null) { 
                button.ChangeBackground(background);
                button.ChangeTextColor(text);
                }
            }
        }

        internal void SetAnswerButtonColors(ushort buttonIndex, Color background, Color text) 
        {
            if (buttonIndex >= AnswerButtons.Length || AnswerButtons[buttonIndex] == null) return;
            AnswerButtons[buttonIndex].ChangeBackground(background);
            AnswerButtons[buttonIndex].ChangeTextColor(text);
        }

        internal void WriteQuestion(Question question, ushort correctAnswerIndex) 
        {
            SetWrongAnswers(question.other_answers, correctAnswerIndex);
            SetCorrectAnswer(correctAnswerIndex, question.correct_answer);
            QuestionLabelText.text = question.questionText;
        }
        #endregion

        #region PRIVATE METHODS
        private void SetCorrectAnswer(int buttonIndex, string answer)
        {
            if (buttonIndex >= AnswerButtons.Length || AnswerButtons[buttonIndex] == null) return;
            AnswerButtons[buttonIndex].SetAnswer(answer, AnswerType.CORRECT);
        }
        private void SetWrongAnswers(string[] answerOptions, ushort correctAnswerIndex)
        {
            ushort current_wrong_answer = 0;
            for (ushort answer = 0; answer < AnswerButtons.Length; ++answer)
            {
                if (answer != correctAnswerIndex && current_wrong_answer < answerOptions.Length)
                {
                    if (AnswerButtons[answer] != null)
                        AnswerButtons[answer].SetAnswer(answerOptions[current_wrong_answer], AnswerType.INCORRECT);
                    current_wrong_answer++;
                }
            }
        }
        #endregion

#if UNITY_EDITOR
        public void CreateEditorVariables() 
        {
            if (AnswerButtons == null || AnswerButtons.Length != Test.AnswerOptionsPerQuestion)
            AnswerButtons = new AnswerButton[Test.AnswerOptionsPerQuestion];
        }
#endif
    }
}