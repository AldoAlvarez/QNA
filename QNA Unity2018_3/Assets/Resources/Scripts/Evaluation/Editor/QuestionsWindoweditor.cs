using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.General;
using AGAC.Evaluation.Tools;
using AGAC.Evaluation.Base;
using AGAC.StandarizedTime.Drawers;

namespace CustomEditors.Evaluation
{
    public class QuestionsWindoweditor
    {
        public void DrawQuestions(SerializedProperty test, SerializedProperty currentQuestion) 
        {
            SetVariables(test);
            if (currentQuestion.intValue < 0||currentQuestion.intValue>=questions.arraySize)
                currentQuestion.intValue = 0;

            DrawQuestionList(currentQuestion);
        }
        #region VARIABLES
        private Color LightBlack = new Color(0.2f, 0.2f, 0.2f);
        private Color DarkGrey = new Color(0.3f, 0.3f, 0.3f);
        private Color LightGrey = new Color(0.4f, 0.4f, 0.4f);
        private Color DarkWhite = new Color(0.6f, 0.6f, 0.6f);
        private Color LightWhite = new Color(0.7f, 0.7f, 0.7f);

        private GUIStyle blackBoldFont;
        private GUIStyle AddQuestionButton;
        private GUIStyle SelectedButtonStyle;
        private GUIStyle NormalButtonStyle;
        private GUIStyle warningButtonStyle;

        private Vector2 questionsScrollPosition;
        private Vector2 questionPropertiesScrollPosition;
        private SerializedProperty questions;
        #endregion

        #region PRIVATE METHODS
        private void DrawQuestionList(SerializedProperty selectedQuestion)
        {
            GUIStyle style = GeneralMethods.CreateBasicGUIStyle(EditorStyles.label, DarkWhite, Color.white);
            EditorGUILayout.BeginVertical(style, GUILayout.Width(550));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Questions", blackBoldFont, GUILayout.Width(100), GUILayout.Height(25));
            int boxHeight = 260;
            GUILayout.Space(10);
            if (GUILayout.Button("Add Question", AddQuestionButton))
                TestCreator.Instance.AddQuestion();
            GUILayout.Space(10);

                    questionsScrollPosition = GUILayout.BeginScrollView(questionsScrollPosition, false, false, GUILayout.Width(185), GUILayout.Height(boxHeight));
            DrawQuestionIDList(selectedQuestion);
                    GUILayout.EndScrollView();

                EditorGUILayout.EndVertical();
            GUILayout.Space(10);
            DrawQuestionProperties(selectedQuestion.intValue);
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        private void DrawQuestionProperties(int currentQuestion) 
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(30);
            GUIStyle style = GeneralMethods.CreateBasicGUIStyle(EditorStyles.label, LightWhite, Color.white);
            EditorGUILayout.BeginVertical(style, GUILayout.Width(250));

            questionPropertiesScrollPosition = GUILayout.BeginScrollView(questionPropertiesScrollPosition, false, false, GUILayout.Width(350), GUILayout.Height(290));
           
            SerializedProperty keywords = questions.GetArrayElementAtIndex(currentQuestion).FindPropertyRelative("keywords");
            string id = GetKeyword(keywords, 0) + ", " + GetKeyword(keywords, 1);
            currentQuestion = Mathf.Clamp(currentQuestion, 0, questions.arraySize);
            try
            {
                SerializedProperty Question = questions.GetArrayElementAtIndex(currentQuestion);
                if (Question != null)
                    EditorGUILayout.PropertyField(Question, new GUIContent(id));
            }
            catch (System.Exception) { }

            GUILayout.Space(140);
            DrawDeleteBox();
            GUILayout.Space(30);
            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
            EditorGUILayout.EndVertical();
        }

        private void DrawDeleteBox() 
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUIStyle style = GeneralMethods.CreateBasicGUIStyle(EditorStyles.label, new Color(1,0.6f,0.2f), Color.white);
            EditorGUILayout.BeginVertical(style);
            GUILayout.Space(30);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(50);
            DrawDeleteButton();
            GUILayout.Space(50);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(30);
            EditorGUILayout.EndVertical();
            GUILayout.Space(20);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawDeleteButton() 
        {
            if (GUILayout.Button("Delete Question", warningButtonStyle, GUILayout.Height(30)))
            {
                bool deleteQuestion = EditorUtility.DisplayDialog("Delete Question.",
              "Are you sure you want to delete this question?",
              "Yes", "No");
                if (deleteQuestion) 
                {
                    QuestionDrawer.PrepareForDestruction();
                    TestCreator.Instance.RemoveQuestion();
                }
            }
        }

        private void DrawQuestionIDList(SerializedProperty selectedQuestion) 
        {
            string id = string.Empty;
            SerializedProperty keywords;
            for (int question = 0; question < questions.arraySize; ++question) 
            {
                keywords = questions.GetArrayElementAtIndex(question).FindPropertyRelative("keywords");
                id =  GetKeyword(keywords, 0) + ", " + GetKeyword(keywords, 1);
                if (question == selectedQuestion.intValue)
                    GUILayout.Label(id, SelectedButtonStyle, GUILayout.Width(150));
                else if (GUILayout.Button(id, NormalButtonStyle, GUILayout.Width(135))) 
                {
                    selectedQuestion.intValue = question;
                    GUI.FocusControl(null);
                }
            }
        }

        private string GetKeyword(SerializedProperty keywords, int index) 
        {
            return keywords.GetArrayElementAtIndex(index).stringValue;
        }
        private void SetVariables(SerializedProperty test)
        {
            questions = test.FindPropertyRelative("questions");
            SetGUIStyles();
        }

        private void SetGUIStyles() 
        {
            blackBoldFont = new GUIStyle(EditorStyles.label);
            blackBoldFont.normal.textColor = Color.black;
            blackBoldFont.fontStyle = FontStyle.BoldAndItalic;
            blackBoldFont.fontSize = 14;

            warningButtonStyle = new GUIStyle(GUI.skin.button);
            warningButtonStyle.normal.background = GeneralMethods.GetTexture(new Color(1f, 0.5f, 0.05f));
            warningButtonStyle.normal.textColor = new Color(1, 1, 0);
            warningButtonStyle.fontStyle = FontStyle.Italic;
            warningButtonStyle.fontSize = 13;

            NormalButtonStyle = GeneralMethods.CreateBasicGUIStyle(EditorStyles.label, LightGrey, Color.white);
            SelectedButtonStyle = GeneralMethods.CreateBasicGUIStyle(EditorStyles.label, LightBlack, Color.white);
            AddQuestionButton = GeneralMethods.CreateBasicGUIStyle(GUI.skin.button, new Color(0.35f, 0.81f, 0.38f), Color.white);
        }
        #endregion
    }
}