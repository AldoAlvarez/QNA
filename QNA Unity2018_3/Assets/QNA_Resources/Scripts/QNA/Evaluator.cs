using System.Collections;
using UnityEngine;
using QNA.ButtonLabel;
using QNA.ButtonLabel.Options;
using General;

namespace QNA
{
    public class Evaluator : MonoBehaviour
    {
        #region UNITY METHODS
        private void Awake()
        {
            if (testsContainer != null) return;
            testsContainer = new TestsContainer();
            //testsContainer.CreateSampleTest();
            //TestLoader.LoadTest(out testsContainer);
        }

        private void FixedUpdate()
        {
            if (!applying_test) return;
            evaluationTimer.FixedUpdate();
            UI.UpdateTme(evaluationTimer.timer);
        }
        #endregion

        #region VARIABLES
        private MinuteTimer evaluationTimer = new MinuteTimer();
        [SerializeField] [Range(0, 5)] private int highlight_duration = 2;

        [SerializeField] private AnswerButtonSettings ButtonSettings = new AnswerButtonSettings();
        [SerializeField] private EvaluationUI UI = new EvaluationUI();
        [SerializeField] UnityEngine.Events.UnityEvent OnFinishedEvaluationAction;

        private static TestsContainer testsContainer;
        private bool applying_test = false;
        private bool can_answer = false;

        private int correctly_answered_answers = 0;

        public Test CurrentTest { get { return testsContainer.all_tests[CurrentTestIndex]; } }
        public ushort CurrentTestIndex = 0;
        public Question CurrentQuestion { get { return CurrentTest.questions[CurrentQuestionIndex]; } }
        private int CurrentQuestionIndex = 0;

        private AnswerButton answeredButton = null;
        private ushort CurrentCorrectAnswerIndex = 0;
        #endregion

        #region PUBLIC METHODS
        public void ResetTest()
        {
            ClearButtons();
            evaluationTimer.Reset();
            correctly_answered_answers = 0;
            CurrentQuestionIndex = 0;
            can_answer = true;
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
            UI.SetButtonsColors(NeutralBackground, highlightenedText);

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
            UI.SetButtonColors(CurrentCorrectAnswerIndex, rightAnsBkgn, textCol);
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
                    ++correctly_answered_answers;
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
            UI.SetButtonsColors(NeutralBkgnd, NormalTxt);
        }

        private void EndEvaluation()
        {
            UI.UpdateResults(evaluationTimer.timer, correctly_answered_answers);
            OnFinishedEvaluationAction.Invoke();
        }

        private IEnumerator PrepareNextQuestion()
        {
            yield return new WaitForSeconds(highlight_duration);
            ClearButtons();

            if (CurrentQuestionIndex >= 5)
                EndEvaluation();
            else
                WriteNextQuestion();
        }
        #endregion

#if UNITY_EDITOR
        [SerializeField] private int SelectedTab = 0;
        public TestCreator testCreator = new TestCreator();

        public void CreateEditorVariables()
        {
            if(testCreator==null)
                testCreator = new TestCreator();

            testCreator.CreateEditorVariables();
            ButtonSettings.CreateSettings();
            UI.CreateEditorVariables();
        }
#endif
    }
}