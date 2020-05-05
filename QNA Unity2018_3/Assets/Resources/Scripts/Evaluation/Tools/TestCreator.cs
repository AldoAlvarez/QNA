using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using AGAC.Evaluation.Base;
using AGAC.General;
using UnityEditor;

namespace AGAC.Evaluation.Tools
{
    [DisallowMultipleComponent]
    [InitializeOnLoad]
    public sealed class TestCreator : MonoBehaviour
    {
        static TestCreator()
        {
            EditorApplication.update += CheckMultipleInstances;
        }
        private void Start()
        {
            CheckMultipleInstances();
        }

        #region VARIABLES
        private static TestCreator instance;
        public static TestCreator Instance { get { SetInstance(); return instance; } }

        [SerializeField]
        private TestsContainer TestsContainer;
        private Test[] Tests { 
            get { return TestsContainer.Tests; }
            set { TestsContainer.Tests = value; }
        }


        [SerializeField] private int currentTest = 0;
        [SerializeField] private int currentQuestion = 0;

        public string TestsFileName = "TestsContainer";
        public string AssetsPath = "Tests/";
        private string fullPath { get { return Application.dataPath + "/Resources/" + AssetsPath; } }
        #endregion

        #region PUBLIC METHODS
        public void SaveTests()
        {
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            string FileContents = JsonUtility.ToJson(TestsContainer);
            File.WriteAllText(fullPath + TestsFileName + ".json", FileContents/*, System.Text.Encoding.UTF7*/);
        }

        public void RemoveTest()
        {
            CreateContainer();
            if (Tests.Length == 1)
                Tests[0] = GetEmptyTest();
            else 
                GeneralMethods.RemoveElementFromArray(currentTest, ref TestsContainer.Tests);
            currentTest = 0;
            currentQuestion = 0;
        }
        public void RemoveQuestion()
        {
            int tempQuestion = currentQuestion;
            currentQuestion = 0;
            CreateContainer();
            if (Tests[currentTest].questions.Length == 1)
                Tests[currentTest].questions[0] = GetEmptyQuestion();
            else
                GeneralMethods.RemoveElementFromArray(tempQuestion, ref TestsContainer.Tests[currentTest].questions);
        }

        public void AddTest()
        {
            CreateContainer();
            if (Tests.Length >= 9) return;

            GeneralMethods.AddElementToArray(ref TestsContainer.Tests);
            currentTest = Tests.Length - 1;
            currentQuestion = 0;
            Tests[currentTest] = GetEmptyTest();
        }
        public void AddQuestion()
        {
            CreateContainer();
            if (Tests[currentTest].questions.Length >= 20) return;

            GeneralMethods.AddElementToArray(ref TestsContainer.Tests[currentTest].questions);
            currentQuestion = Tests[currentTest].questions.Length - 1;
            Tests[currentTest].questions[currentQuestion] = GetEmptyQuestion();
        }

        public void CreateSampleTest()
        {
            if(TestsContainer==null)
                TestsContainer = new TestsContainer();
            if (Tests == null || Tests.Length <= 0)
                Tests = new Test[1] { GetSampleTest() };

            SaveTests();
        }
        public void LoadSavedTest()
        {
            TestLoader.LoadTest(out TestsContainer);
            CreateSampleTest();
        }
        #endregion

        #region PRIVATE METHODS
        #region instance
        private static void CheckMultipleInstances()
        {
            SetInstance();

            TestCreator[] instances = GameObject.FindObjectsOfType<TestCreator>();
            foreach (TestCreator _instance in instances)
                if (_instance != instance)
                    DestroyImmediate(_instance);
        }
        private static void SetInstance()
        {
            if (instance != null) return;
            instance = GeneralMethods.GetInstance<TestCreator>("Test Creator");
            instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
        }
        #endregion

        private Test GetEmptyTest() 
        {
            Test test = new Test("New Test", 0);
            test.questions = new Question[1] { GetEmptyQuestion() };
            return test;

        }
        private Question GetEmptyQuestion() 
        {
            Question question = new Question();
            question.AnswerValue = 1;
            question.questionText = string.Empty;
            question.correct_answer = string.Empty;
            for(int answer = 0;answer<question.other_answers.Length;++answer)
                question.other_answers[answer] = string.Empty;
            return question;
        }
        private Test GetSampleTest() 
        {
            Test test = new Test("Sample Test", 30);
            test.questions = new Question[5];
            for (int i = 0; i < 5; ++i)
            {
                test.questions[i] = new Question();
                test.questions[i] = GetSampleQustion((short)(i + 1));
            }
            return test;
        }
        private Question GetSampleQustion(short questionIndex = 0)
        {
            Question question = new Question();
            question.AnswerValue = 10;
            question.questionText = "This is sample question number " + questionIndex.ToString() + ". To get full credits, you must mark as answer the question number.";
            question.correct_answer = questionIndex.ToString();

            uint TotalAlternativeOptions = Test.AnswerOptionsPerQuestion - 1;
            question.other_answers = new string[TotalAlternativeOptions];
            for (int option = 0; option < TotalAlternativeOptions; ++option)
                question.other_answers[option] = (questionIndex + option + 1).ToString();

            return question;
        }

        private void CreateContainer()
        {
            if (TestsContainer == null)
                TestsContainer = new TestsContainer();
            if (TestsContainer.Tests == null)
            {
                TestsContainer.Tests = new Test[1];
                CreateSampleTest();
            }
        }
        #endregion

#if UNITY_EDITOR
        public void CreateEditorVariables()
        {
            CreateContainer();
        }
#endif
    }
}