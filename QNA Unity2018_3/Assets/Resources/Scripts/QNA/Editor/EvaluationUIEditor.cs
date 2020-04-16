using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CustomEditors.QNA
{
    public class EvaluationUIEditor
    {
        #region PUBLIC EDITOR METHODS
        public void OnEnable(SerializedProperty EvaluationUI)
        {
            QuestionLabelText = EvaluationUI.FindPropertyRelative("QuestionLabelText");
            AnswerButtons = EvaluationUI.FindPropertyRelative("AnswerButtons");
            CurrentTime = EvaluationUI.FindPropertyRelative("CurrentTime");

            TotalTime = EvaluationUI.FindPropertyRelative("TotalTime");
            CorrectAnswers = EvaluationUI.FindPropertyRelative("CorrectAnswers");
        }

        public void OnInspectorGUI() 
        {
            DrawMainInterfaceVariables();
            GUILayout.Space(6);
            DrawResultsInterfaceVariables();
        }
        #endregion

        #region VARIABLES
        private SerializedProperty QuestionLabelText;
        private SerializedProperty AnswerButtons;
        private SerializedProperty CurrentTime;

        private SerializedProperty TotalTime;
        private SerializedProperty CorrectAnswers;
        #endregion

        private void DrawMainInterfaceVariables() 
        {

            DrawInterfaceHeader("Evaluation");
            EditorGUI.indentLevel++;
            GUILayout.Space(5);

            GUIContent label = new GUIContent("Question Label:", "The UI text reference to the label where the question will be displayed.");
            DrawVariable(ref QuestionLabelText, label);

            label = new GUIContent("Timer Display:", "The UI text reference to the label where the current time spent will be displayed.");
            DrawVariable(ref CurrentTime, label);

            EditorGUILayout.LabelField("Answer Buttons", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            for (int button = 0; button < AnswerButtons.arraySize; ++button) 
            {
                EditorGUILayout.PropertyField(
                    AnswerButtons.GetArrayElementAtIndex(button),
                    new GUIContent("Button " + button.ToString()),
                      GUILayout.MaxWidth(EditorVariables.LargePropertyidth));
            }
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }

        private void DrawResultsInterfaceVariables() 
        {
            DrawInterfaceHeader("Results");
            GUILayout.Space(5);
            EditorGUI.indentLevel++;
            GUIContent label = new GUIContent("Total Time:", "The UI text reference to the label where the total duration time of the test will be displayed.");
            DrawVariable(ref TotalTime, label);
            label = new GUIContent("Correct Answers:", "The UI text reference to the label where the number of answers answered correctly will be displayed.");
            DrawVariable(ref CorrectAnswers, label);
            EditorGUI.indentLevel--;
        }

        private void DrawInterfaceHeader(string header) 
        {
            GUIStyle boldLargeFont = EditorStyles.boldLabel;
            boldLargeFont.fontSize = 14;
            EditorGUILayout.LabelField(header, boldLargeFont, GUILayout.Height(20));
            boldLargeFont.fontSize = 11;
        }

        private void DrawVariable(ref SerializedProperty property, GUIContent label) 
        {
            EditorGUILayout.PropertyField(property, label, GUILayout.MaxWidth(EditorVariables.LargePropertyidth));
        }
    }
}