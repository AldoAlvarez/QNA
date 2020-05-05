using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.General;
using AGAC.Evaluation.Tools;
using AGAC.StandarizedTime.Drawers;

namespace CustomEditors.Evaluation
{
    public class TestVariablesWindowEditor
    {
        public void DrawTest(SerializedProperty test)
        {
            SetTestVariables(test);

            GUIStyle style = GeneralMethods.CreateBasicGUIStyle(EditorStyles.label, DarkWhite, Color.white);
            EditorGUILayout.BeginVertical(style, GUILayout.Width(200));
            DrawTestVariables();
            GUILayout.Space(70);
            CreateWarningBox();
            GUILayout.Space(25);
            EditorGUILayout.EndVertical();
        }

        #region VARIABLES
        private Color DarkWhite = new Color(0.55f, 0.55f, 0.55f);
        private GUIStyle warningButtonStyle;

        private SerializedProperty passingScore;
        private SerializedProperty TestID;
        private SerializedProperty questions;

        private SerializedProperty lastTest;
        #endregion

        #region PRIVATE METHODS
        private void DrawTestVariables() 
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);

            EditorGUILayout.BeginVertical();
            EditorGUIUtility.labelWidth = 100;
            GUILayout.Space(30);
            TestID.stringValue = EditorGUILayout.DelayedTextField(new GUIContent("ID / Name:"), TestID.stringValue, GUILayout.MaxWidth(180));
            GUILayout.Space(15);
            EditorGUI.BeginDisabledGroup(true);
            int max_score = GetMaximumtestScore();
            EditorGUILayout.IntField("Max. Score:", max_score, GUILayout.MaxWidth(180));
            EditorGUI.EndDisabledGroup();
            int value = EditorGUILayout.IntField("Passing Score", passingScore.intValue, GUILayout.MaxWidth(180));
            passingScore.intValue = Mathf.Clamp(value, 0, max_score);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }
        private void CreateWarningBox() 
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUIStyle warningBox = GeneralMethods.CreateBasicGUIStyle(EditorStyles.label, new Color(0.9f, 0.5f, 0.4f), Color.white);
            EditorGUILayout.BeginVertical(warningBox, GUILayout.Width(200));
            GUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(30);
            DrawDeleteButton();
            GUILayout.Space(30);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);
            EditorGUILayout.EndVertical();

            GUILayout.Space(20);
            EditorGUILayout.EndHorizontal();
        }
        private void DrawDeleteButton() 
        {
            if (GUILayout.Button("Delete Test", warningButtonStyle, GUILayout.Height(30)))
            {
                bool deleteTest = EditorUtility.DisplayDialog("Delete Test.",
              "Are you sure you want to delete test " + TestID.stringValue + "?",
              "Yes", "No");
                if (deleteTest) 
                {
                    QuestionDrawer.PrepareForDestruction();
                    TestCreator.Instance.RemoveTest();
                }
            }
        }

        private int GetMaximumtestScore()
        {
            int score = 0;
            for (int question = 0; question < questions.arraySize; ++question)
                score += questions.GetArrayElementAtIndex(question).FindPropertyRelative("AnswerValue").intValue;
            return score;
        }
        private void SetTestVariables(SerializedProperty test)
        {
            if (lastTest==test) return;
            if (test == null) return;
            TestID = test.FindPropertyRelative("TestID");
            passingScore = test.FindPropertyRelative("passingScore");
            questions = test.FindPropertyRelative("questions");
            lastTest = test;
            CreateGUIStyles();
        }
        private void CreateGUIStyles()
        {
            warningButtonStyle = new GUIStyle(GUI.skin.button);
            warningButtonStyle.normal.background = GeneralMethods.GetTexture(new Color(0.6f, 0.05f, 0.05f));
            warningButtonStyle.normal.textColor = new Color(1, 1, 0);
            warningButtonStyle.fontStyle = FontStyle.Italic;
            warningButtonStyle.fontSize = 13;
        }
        #endregion
    }
}