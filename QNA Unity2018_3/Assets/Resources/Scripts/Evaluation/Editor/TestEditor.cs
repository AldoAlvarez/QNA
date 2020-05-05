using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CustomEditors.Evaluation
{
    public class TestEditor
    {
        public void OnEnable(SerializedProperty Test)
        {
            SetTestVariables(Test);
        }

        public void DrawQuestions() 
        {
            for (int i = 0; i < questions.arraySize; ++i)
                DrawQuestion(questions.GetArrayElementAtIndex(i));
        }


        #region VARIABLES
        private SerializedProperty questions;
        private SerializedProperty TestID;
        private SerializedProperty passingScore;


        private SerializedProperty AnswerValue;
        private SerializedProperty questionText;
        private SerializedProperty correct_answer;
        private SerializedProperty other_answers;

        private static bool[] displayedQuestions;
        #endregion

        private void DrawQuestion(SerializedProperty question) 
        {
            SetQuestionVariables(question);
        }

        private void SetTestVariables(SerializedProperty test) 
        {
            questions = test.FindPropertyRelative("questions");
            TestID = test.FindPropertyRelative("TestID");
            passingScore = test.FindPropertyRelative("passingScore");
        }

        private void SetQuestionVariables(SerializedProperty question) 
        {
            AnswerValue = question.FindPropertyRelative("AnswerValue");
            questionText = question.FindPropertyRelative("questionText");
            correct_answer = question.FindPropertyRelative("correct_answer");
            other_answers = question.FindPropertyRelative("other_answers");
        }
    }
}