using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.General;
using AGAC.Evaluation.Tools;
using AGAC.Evaluation.Base;

namespace CustomEditors.Evaluation
{
    public class TestCreatorWindow : EditorWindow
    {
        [MenuItem("Window/Test Creator")]
        public static void DisplayWindow()
        {
            TestCreatorWindow window = (TestCreatorWindow)EditorWindow.GetWindow(typeof(TestCreatorWindow));
            window.minSize = new Vector2(1030, 350);
            window.maxSize = new Vector2(1035, 355);
            window.Show();
            PrepareEssentials();
        }

        #region WINDOW METHODS
        private void OnGUI()
        {
            if (myObject == null)
            {
                TestCreatorWindow window = (TestCreatorWindow)EditorWindow.GetWindow(typeof(TestCreatorWindow));
                window.Close();
                return;
            }

            SetEditorVariables();
            if (!createdvariables) return;
            myObject.Update();

            if (currentTest.intValue < 0 || currentTest.intValue >= Tests.arraySize)
                currentTest.intValue = 0;

            DrawBackground();

            float lableWidth = EditorGUIUtility.labelWidth;

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            DrawTestsList();
            GUILayout.Space(10);
            DrawTestData();
            GUILayout.Space(20);
            DrawQuestions();
            GUILayout.Space(25);
            EditorGUILayout.EndHorizontal();

            EditorGUIUtility.labelWidth = lableWidth;
            myObject.ApplyModifiedProperties();
        }
        private void OnDestroy()
        {
            if (myObject != null)
            {
                bool save_data = EditorUtility.DisplayDialog("Save tests.",
               "Do you want to save the changes made to the tests?",
               "Yes", "No");

                if (save_data)
                    TestCreator.Instance.SaveTests();
            }
        }
        #endregion

        #region VARIABLES
        private Texture2D backgroundTexture;

        private Vector2 TestsScrollPosition = Vector2.zero;
        private GUIStyle boldWhiteFont;

        private GUIStyle SelectedButtonStyle;
        private GUIStyle NormalButtonStyle;
        private GUIStyle AddTestButton;

        private Color LightBlack = new Color(0.2f, 0.2f, 0.2f);
        private Color DarkGrey = new Color(0.3f, 0.3f, 0.3f);
        private Color LightGrey = new Color(0.4f, 0.4f, 0.4f);
        private Color DarkWhite = new Color(0.6f, 0.6f, 0.6f);

        private static SerializedObject myObject;
        private SerializedProperty currentTest;
        private SerializedProperty currentQuestion;
        private SerializedProperty TestsContainer;
        private SerializedProperty Tests;

        private TestVariablesWindowEditor testEditor;
        private QuestionsWindoweditor questionsEditor;

        private static bool createdvariables = false;
        #endregion

        #region PRIVATE METHODS
        #region draw methods
        private void DrawTestsList()
        {
            GUIStyle style = GeneralMethods.CreateBasicGUIStyle(EditorStyles.label, DarkWhite, Color.white);
            EditorGUILayout.BeginVertical(style, GUILayout.Width(140));
            EditorGUILayout.LabelField("Tests", boldWhiteFont, GUILayout.Width(100), GUILayout.Height(25));
            int boxHeight = 200;
            GUILayout.Space(10);
            if (GUILayout.Button("Add Test", AddTestButton))
                TestCreator.Instance.AddTest();
            GUILayout.Space(10);
            TestsScrollPosition = GUILayout.BeginScrollView(TestsScrollPosition, false, false, GUILayout.MaxWidth(140), GUILayout.Height(boxHeight));
            DrawTestIDsList();
            GUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }
        private void DrawTestData()
        {
            testEditor.DrawTest(GetCurrentTest());
        }
        private void DrawQuestions() 
        {
            questionsEditor.DrawQuestions(GetCurrentTest(), currentQuestion);
        }

        private void DrawTestIDsList()
        {
            string[] Tests_IDs = GetTestIDs();
            for (int id = 0; id < Tests_IDs.Length; ++id)
            {
                if (id == currentTest.intValue)
                    GUILayout.Label(new GUIContent(Tests_IDs[id]), SelectedButtonStyle, GUILayout.MaxWidth(130));
                else if (GUILayout.Button(new GUIContent(Tests_IDs[id]), NormalButtonStyle, GUILayout.MaxWidth(115))) 
                {
                    currentTest.intValue = id;
                    currentQuestion.intValue = 0;
                    GUI.FocusControl(null);
                }
            }
        }
        private void DrawBackground()
        {
            GUI.DrawTexture(new Rect(0, 0, position.width + 50, position.height + 50), backgroundTexture, ScaleMode.StretchToFill);
        }
        #endregion

        #region getters & setters
        private string[] GetTestIDs()
        {
            List<string> test_ids = new List<string>();
            for (int test = 0; test < Tests.arraySize; ++test)
            {
                test_ids.Add(Tests.GetArrayElementAtIndex(test).FindPropertyRelative("TestID").stringValue);
            }
            return test_ids.ToArray();
        }
        private SerializedProperty GetCurrentTest()
        {
            return Tests.GetArrayElementAtIndex(currentTest.intValue);
        }
        private static void PrepareEssentials()
        {
            SerializedObject obj = new SerializedObject(TestCreator.Instance);
            if (obj != myObject)
            {
                myObject = obj;
                createdvariables = false;
                TestCreator.Instance.LoadSavedTest();
            }
        }
        private void SetEditorVariables()
        {
            if (createdvariables) return;

            currentTest = myObject.FindProperty("currentTest");
            currentQuestion = myObject.FindProperty("currentQuestion");
            TestsContainer = myObject.FindProperty("TestsContainer");
            Tests = TestsContainer.FindPropertyRelative("Tests");

            if (currentTest.intValue < 0 || currentTest.intValue >= Tests.arraySize) 
                currentTest.intValue = 0;

            backgroundTexture = GeneralMethods.GetTexture(DarkGrey);
            SetGUIStyles();

            testEditor = new TestVariablesWindowEditor();
            questionsEditor = new QuestionsWindoweditor();
            createdvariables = true;
        }
        private void SetGUIStyles() 
        {
            boldWhiteFont = new GUIStyle(EditorStyles.label);
            boldWhiteFont.normal.textColor = Color.white;
            boldWhiteFont.fontStyle = FontStyle.BoldAndItalic;
            boldWhiteFont.fontSize = 14;

            NormalButtonStyle = GeneralMethods.CreateBasicGUIStyle(EditorStyles.label, LightGrey, Color.white);
            SelectedButtonStyle = GeneralMethods.CreateBasicGUIStyle(EditorStyles.label, LightBlack, Color.white);
            AddTestButton = GeneralMethods.CreateBasicGUIStyle(GUI.skin.button, new Color(0.35f, 0.81f, 0.38f), Color.white);
        }
        #endregion
        #endregion
    }
}