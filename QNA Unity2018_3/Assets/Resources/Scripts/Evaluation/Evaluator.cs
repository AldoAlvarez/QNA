using System.Collections;
using UnityEngine;
using AGAC.StandarizedTime;
using AGAC.Evaluation.ButtonLabel;
using AGAC.Evaluation.ButtonLabel.Options;
using AGAC.Evaluation.Base;

namespace AGAC.Evaluation
{
    [RequireComponent(typeof(StandarizedTimeCounter))]
    public class Evaluator : MonoBehaviour
    {
        #region UNITY METHODS
        private void Awake()
        {
            if (testsContainer != null) return;
            testsContainer = EvaluationTools.LoadTests();
            timeCounter = GetComponent<StandarizedTimeCounter>();
        }

        private void FixedUpdate()
        {
            if (!applying_test) return;
            UI.UpdateTme(timeCounter.GetCurrentTime());
        }
        #endregion

        #region VARIABLES
        private StandarizedTimeCounter timeCounter;
        [SerializeField] [Range(0, 5)] private int answer_highlight_duration = 2;

        [SerializeField] private AnswerButtonSettings ButtonSettings = new AnswerButtonSettings();
        [SerializeField] private EvaluationUI UI = new EvaluationUI();
        [SerializeField] UnityEngine.Events.UnityEvent OnFinishedEvaluationAction;

        private static TestsContainer testsContainer;
        private bool applying_test = false;
        private bool can_answer = false;

        private int correctly_answered_questions = 0;

        public Test[] AllTests { get { return testsContainer.Tests; } }
        public Test CurrentTest { get { return AllTests[CurrentTestIndex]; } }
        public ushort CurrentTestIndex = 0;
        public Question CurrentQuestion { get { return CurrentTest.questions[CurrentQuestionIndex]; } }
        private int CurrentQuestionIndex = 0;

        private AnswerButton answeredButton = null;
        private ushort CurrentCorrectAnswerIndex = 0;
        #endregion

        #region PUBLIC METHODS
        public void Quit() 
        {
            General.GeneralMethods.CloseApp();
        }
        public void ResetTest()
        {
            ClearButtons();
            timeCounter.Restart();
            correctly_answered_questions = 0;
            CurrentQuestionIndex = 0;
            can_answer = false;
            applying_test = false;
        }
        public void StartEvaluation()
        {
            applying_test = true;
            ClearButtons();
            WriteNextQuestion();
        }

        public void CheckAnswer(GameObject buttonObject)
        {
            answeredButton = buttonObject.GetComponent<AnswerButton>();
            if (answeredButton == null) return;
            if (!can_answer) return;

            can_answer = false;

            Color NeutralBackground = ButtonSettings.GetSettingColor(Settings.Background, (int)Background.Neutral);
            Color highlightenedText = ButtonSettings.GetSettingColor(Settings.Text, (int)Text.Highlightened);
            UI.SetAnswerButtonsColors(NeutralBackground, highlightenedText);

            CallActionsDependingOnAnswer();
            PaintCorrectAnswer(highlightenedText);
            answeredButton = null;
            StartCoroutine(PrepareNextQuestion());
        }
        #endregion

        #region PRIVATE METHODS
        private void PaintCorrectAnswer(Color textCol)
        {
            Color rightAnsBkgn = ButtonSettings.GetSettingColor(Settings.Background, (int)Background.Right_Answer);
            UI.SetAnswerButtonColors(CurrentCorrectAnswerIndex, rightAnsBkgn, textCol);
        }
        private void PaintWrongAnswer()
        {
            Color wrongAnsBkgnd = ButtonSettings.GetSettingColor(Settings.Background, (int)Background.Wrong_Answer);
            answeredButton.ChangeBackground(wrongAnsBkgnd);
        }

        private void CallActionsDependingOnAnswer()
        {
            switch (answeredButton.AnswerType)
            {
                case AnswerType.CORRECT:
                    ++correctly_answered_questions;
                    break;
                case AnswerType.INCORRECT:
                    PaintWrongAnswer();
                    break;
                default: break;
            }
        }

        private void WriteNextQuestion()
        {
            SetCorrectAnswer();
            UI.WriteQuestion(CurrentQuestion, CurrentCorrectAnswerIndex);
            CurrentQuestionIndex++;
            can_answer = true;
        }

        private void SetCorrectAnswer()
        {
            int totalAnswerOptions = CurrentQuestion.other_answers.Length + 1;
            CurrentCorrectAnswerIndex = (ushort)Random.Range(0, totalAnswerOptions);
        }

        private void ClearButtons()
        {
            UI.ClearButtonsText();
            Color NeutralBkgnd = ButtonSettings.GetSettingColor(Settings.Background, (int)Background.Neutral);
            Color NormalTxt = ButtonSettings.GetSettingColor(Settings.Text, (int)Text.Normal);
            UI.SetAnswerButtonsColors(NeutralBkgnd, NormalTxt);
        }

        private void EndEvaluation()
        {
            UI.UpdateResults(timeCounter.GetCurrentTime(), correctly_answered_questions);
            OnFinishedEvaluationAction.Invoke();
        }

        private IEnumerator PrepareNextQuestion()
        {
            yield return new WaitForSeconds(answer_highlight_duration);
            ClearButtons();

            if (CurrentQuestionIndex >= 5)
                EndEvaluation();
            else
                WriteNextQuestion();
        }
        #endregion

#if UNITY_EDITOR
        [SerializeField] private int SelectedTab = 0;

        public void CreateEditorVariables()
        {
            ButtonSettings.CreateSettings();
            UI.CreateEditorVariables();
        }
#endif
    }
}