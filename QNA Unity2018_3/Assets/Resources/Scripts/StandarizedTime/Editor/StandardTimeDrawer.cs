using UnityEngine;
using UnityEditor;
using AGAC.StandarizedTime.Operations;

namespace AGAC.StandarizedTime.Drawers
{
    [CustomPropertyDrawer(typeof(StandardTime))]
    public class StandardTimeDrawer : PropertyDrawer
    {
        #region DRAWER METHODS
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SetDrawerVariables(property);

            Rect PropertyHeaderPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            if (displayed.boolValue)
            {
                float labelWidth = EditorGUIUtility.labelWidth;
                DrawTimeValues(PropertyHeaderPosition);
                EditorGUIUtility.labelWidth = labelWidth;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 40.0f;
        }

        #endregion

        #region VARIABLES
        private SerializedProperty displayed;
        private SerializedProperty TimeValues;

        private const int HeightToField = 0;
        private const int HeightToLabels = 20;

        private GUIStyle AddTimeIcon;
        private bool buttonIsPressed = false;
        #endregion

        #region PRIVATE METHODS
        private void DrawTimeValues(Rect position) 
        {
            int maxTimeValue = StandardTimeVerifyer.GetMaximumTime(0);
            for (int value = 0; value < TimeValues.arraySize; ++value)
            {            
                float fieldHeight = 15;
                DrawValueField(position, value, fieldHeight);
                if (value < TimeValues.arraySize - 1)
                    DrawTimeSeparation(position, value, fieldHeight);
                DrawValueLabel(position, value, fieldHeight);
            }
        }

        private void DrawValueField(Rect position, int valueIndex, float fieldHeight) 
        {
            float positionX = position.x  + (60 * valueIndex);
            Rect TimeValuePos = new Rect(positionX, position.y + HeightToField, 40, fieldHeight);
            int maxTimeValue = StandardTimeVerifyer.GetMaximumTime((StandardTimeValues)valueIndex);
            DrawTimeValueField(TimeValuePos, valueIndex, maxTimeValue);
        }
        private void DrawValueLabel(Rect position, int valueIndex, float fieldHeight) 
        {
            float positionX = position.x + 8 + (60 * valueIndex);
            Rect TimeValuePos = new Rect(positionX, position.y + HeightToLabels, 50, fieldHeight);
            DrawTimeValueLabel(TimeValuePos, valueIndex);
        }

        private void DrawTimeSeparation(Rect position, int valueIndex, float labelHeight) 
        {
            EditorGUIUtility.labelWidth = 100;
            float positionX = position.x + 47 + (60 * valueIndex);
            Rect newPos = new Rect(positionX, position.y, 50, labelHeight);
            EditorGUI.LabelField(newPos, new GUIContent(":"));
        }
        private void DrawTimeValueField(Rect position, int index, int maxValue) 
        {
            EditorGUIUtility.labelWidth = 0;
            EditorGUI.IntSlider(position, TimeValues.GetArrayElementAtIndex(index), 0, maxValue, GUIContent.none);
        }
        private void DrawTimeValueLabel(Rect position, int valueIndex)
        {
            EditorGUIUtility.labelWidth = 50;
            EditorGUI.LabelField(position, new GUIContent(StandardTime.TimeValuesAbreviations[valueIndex]));
        }
        private void SetDrawerVariables(SerializedProperty property) 
        {
            displayed = property.FindPropertyRelative("displayed");
            TimeValues = property.FindPropertyRelative("TimeValues");

            AddTimeIcon = new GUIStyle();
            AddTimeIcon.fontSize = 10;
            AddTimeIcon.normal.background = (Texture2D)Resources.Load("button") as Texture2D;
            AddTimeIcon.focused.background = (Texture2D)Resources.Load("button") as Texture2D;
            AddTimeIcon.hover.background = (Texture2D)Resources.Load("button") as Texture2D;
            AddTimeIcon.active.background = (Texture2D)Resources.Load("button") as Texture2D;
            AddTimeIcon.fontStyle = FontStyle.Normal;
        }
        #endregion
    }
}