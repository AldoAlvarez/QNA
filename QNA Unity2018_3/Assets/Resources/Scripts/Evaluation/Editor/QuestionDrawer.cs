using UnityEngine;
using UnityEditor;
using AGAC.Evaluation.Base;
using AGAC.Evaluation.ButtonLabel.Options;

namespace AGAC.StandarizedTime.Drawers
{
    [CustomPropertyDrawer(typeof(Question))]
    public class QuestionDrawer : PropertyDrawer
    {
        #region DRAWER METHODS
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SetDrawerVariables(property);

            position.height = 20;
            displayed.boolValue = EditorGUI.Foldout(position, displayed.boolValue, label, true);
            if (displayed.boolValue)
            {
                position.y += 10;
                float labelWidth = EditorGUIUtility.labelWidth;
                DrawQuestionProperties(position);
                EditorGUIUtility.labelWidth = labelWidth;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (willBeDestructed) return 15f;

            if (displayed != null && displayed.boolValue)
            {
                uint TotalAlternativeOptions = Test.AnswerOptionsPerQuestion - 1;
                return 260.0f + (TotalAlternativeOptions * 50f);
            }
            else
                return 15.0f;
        }
        #endregion

        #region VARIABLES
        private const int HeightToField = 0;
        private const int HeightToLabels = 20;

        private GUIStyle wrapedTextArea;
        private GUIStyle boldLabel;

        private SerializedProperty displayed;
        private SerializedProperty AnswerValue;
        private SerializedProperty keywords;

        private SerializedProperty questionText;
        private SerializedProperty correct_answer;
        private SerializedProperty other_answers;

        private static bool willBeDestructed = false;
        #endregion

        public static void PrepareForDestruction() 
        {
            willBeDestructed = true;
        }

        #region PRIVATE METHODS
        private void DrawQuestionProperties(Rect position) 
        {
            position.width -= 10;

            DrawAnswerValue(position);
            position.y += 46;
            DrawQuestionText(position);
            position.width -= 30;
            DrawKeywords(position);
            position.y += 40;
            DrawAnswers(position);
        }
        private void DrawKeywords(Rect position) 
        {
            position.y += 45;
            position.height = 20;
            DrawHeader(position, "Keywords");
            EditorGUIUtility.labelWidth = 100;
            position.y += 20;
            position.x += 30;
            DrawKeyword(position, 0);
            position.y += 26;
            DrawKeyword(position, 1);
        }
        private void DrawQuestionText(Rect position)
        {
            DrawTextArea(position, questionText, new GUIContent("Question Text"));
        }

        private void DrawAnswers(Rect position) 
        {
            DrawAnswersHeader(position);
            position.x += 30;
            DrawCorrectAnswer(position);
            Rect divisionPosition = position;
            divisionPosition.y += 155;
            divisionPosition.x -= 30;
            DrawDivision(divisionPosition);
            DrawOtherAnswers(position);
        }

        private void DrawKeyword(Rect position, int index) 
        {
            string keyword =  keywords.GetArrayElementAtIndex(index).stringValue;
            keyword = EditorGUI.TextField(position, new GUIContent("keyword "+(index+1).ToString()), keyword);
            string[] total_text = keyword.Split();
            int total_letters = total_text[0].Length > 9 ? 9 : total_text[0].Length;
            string newKeyword = string.Empty;
            for (int letter = 0; letter < total_letters; ++letter)
                newKeyword += total_text[0][letter];
            keywords.GetArrayElementAtIndex(index).stringValue = newKeyword;
        }
        private void DrawAnswersHeader(Rect position) 
        {
            position.y += 90;
            position.height = 20;
            DrawHeader(position, "Answers");
        }
        private void DrawCorrectAnswer(Rect position) 
        {
            position.y += 110;
            DrawTextArea(position, correct_answer, new GUIContent("Correct"));
        }
        private void DrawDivision(Rect position) 
        {
            position.width += 30;
            EditorGUI.LabelField(position, string.Empty, GUI.skin.horizontalSlider);
        }

        private void DrawOtherAnswers(Rect position)
        {
            uint TotalAlternativeOptions = Test.AnswerOptionsPerQuestion - 1;
            position.y += 175;
            Rect newPosition = position;
            for (int answer = 0; answer < TotalAlternativeOptions; ++answer) 
            {
                newPosition.y = position.y + (50 * answer);
                DrawOtherAnswer(newPosition, answer);
            }
        }
        private void DrawOtherAnswer(Rect position, int answerIndex)
        {
            GUIContent label = new GUIContent("Option " + (answerIndex + 1).ToString());
            SerializedProperty answerPropery = other_answers.GetArrayElementAtIndex(answerIndex);
            DrawTextArea(position, answerPropery, label, 2);
        }

        private void DrawTextArea(Rect position, SerializedProperty property, GUIContent label, uint lines = 3) 
        {
            EditorGUI.LabelField(position, label);
            position.x += 100;
            position.width -= 100;
            position.height = 14*lines;

            property.stringValue = EditorGUI.TextArea(position, property.stringValue, wrapedTextArea);
        }

        private void DrawAnswerValue(Rect position) 
        {
            EditorGUIUtility.labelWidth = 60;
            position.y += position.height;
            AnswerValue.intValue = EditorGUI.IntSlider(position, new GUIContent("Value"), AnswerValue.intValue, 0, 10);
        }
        private void DrawHeader(Rect position, string text) 
        {
            EditorGUI.LabelField(position, text, boldLabel);
        }

        private void SetDrawerVariables(SerializedProperty question)
        {
            AnswerValue = question.FindPropertyRelative("AnswerValue");
            displayed = question.FindPropertyRelative("displayed");
            keywords = question.FindPropertyRelative("keywords");

            questionText = question.FindPropertyRelative("questionText");
            correct_answer = question.FindPropertyRelative("correct_answer");
            other_answers = question.FindPropertyRelative("other_answers");

            willBeDestructed = false;

            wrapedTextArea = new GUIStyle(EditorStyles.textArea);
            wrapedTextArea.wordWrap = true;
            boldLabel = new GUIStyle(EditorStyles.label);
            boldLabel.fontStyle = FontStyle.Bold;
        }
        #endregion
    }
}