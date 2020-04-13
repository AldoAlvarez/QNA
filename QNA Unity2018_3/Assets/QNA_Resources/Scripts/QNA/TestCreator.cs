using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace QNA
{
    [System.Serializable]
    public class TestCreator
    {
        #region VARIABLES
        public TestsContainer TestsContainer;

        [SerializeField] private int currentTest = 0;
        [SerializeField] private int currentQuestion = 0;

        public static string TestsFileName = "TestsContainer";
        public static string AssetsPath = "/Tests/";
        private static string fullPath { get { return Application.dataPath + "/Resources" + AssetsPath; } }
        #endregion

        #region
        public static void SaveTests(TestsContainer testsContainer) 
        {
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            string FileContents = JsonUtility.ToJson(testsContainer);
            File.WriteAllText(fullPath + TestsFileName, FileContents, System.Text.Encoding.UTF7);
        }

        public void RemoveTest(int index) 
        {
            CreateContainer();
            RemoveElementFromArray<Test>(index, ref TestsContainer.all_tests);
        }
        public void RemoveQuestion(int index)
        {
            CreateContainer();
            RemoveElementFromArray<Test>(index, ref TestsContainer.all_tests);
        }

        public void AddTest()
        {
            CreateContainer();
            if (TestsContainer.all_tests.Length >= 9) return;

            AddElementToArray<Test>(ref TestsContainer.all_tests);
            TestsContainer.all_tests[currentTest] = new Test("New Test", 0);
        }
        public void AddQuestion() 
        {
            CreateContainer();
            if (TestsContainer.all_tests[currentTest].questions.Length >= 20) return;

            AddElementToArray<Question>(ref TestsContainer.all_tests[currentTest].questions);
            TestsContainer.all_tests[currentTest].questions[currentQuestion] = new Question();
        }

        public void CreateSampleTest() 
        {
            CreateContainer();
            TestsContainer.all_tests = new Test[1];
            Test test = new Test("Sample Test", 30);
            test.questions = new Question[5];
            for (int i = 0; i < 5; ++i)
            {
                test.questions[i] = new Question();
                test.questions[i].SetSampleQustion((short)(i + 1));
            }
            TestsContainer.all_tests[0] = test;
            SaveTests(TestsContainer);
        }
        #endregion

        #region PRIVATE METHODS
        private void AddElementToArray<T>(ref T[]array) where T : class
        {
            T[] new_array = new T[array.Length + 1];
            array.CopyTo(new_array, 0);

            array = new T[new_array.Length];
            new_array.CopyTo(array, 0);
        }

        private void RemoveElementFromArray<T>(int index, ref T[] array) 
        {
            if (index < 0 || index >= array.Length) return;

            T[] new_array = new T[array.Length - 1];

            for (int i = 0; i < index; ++i)
                new_array[i] = array[i];
            for (int i = index + 1; i < array.Length; ++i)
                new_array[i - 1] = array[i];

            for (int i = 0; i < new_array.Length; ++i)
                array[index] = new_array[i];
        }

        private void CreateContainer() 
        {
            if (TestsContainer == null)
                TestsContainer = new TestsContainer();
            if (TestsContainer.all_tests == null) 
            {
                TestsContainer.all_tests = new Test[1];
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