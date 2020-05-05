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
        #endregion

        #region PRIVATE METHODS
        private void DrawTimeValues(Rect position) 
        {
            position.height = 15;
            for (int value = 0; value < TimeValues.arraySize; ++value)
            {
                DrawValueField(position, value);
                if (value < TimeValues.arraySize - 1)
                    DrawTimeSeparation(position, value);
                DrawValueLabel(position, value);
            }
        }

        private void DrawValueField(Rect position, int valueIndex) 
        {
            float positionX = position.x  + (50 * valueIndex);
            float positionY = position.y + (HeightToField* valueIndex);
            Rect TimeValuePos = new Rect(positionX, positionY, 36, position.height);

            int maxTimeValue = StandardTimeVerifyer.GetMaximumTime((StandardTimeValues)valueIndex);
            DrawTimeValueField(TimeValuePos, valueIndex, maxTimeValue);
        }
        private void DrawValueLabel(Rect position, int valueIndex) 
        {
            float positionX = position.x + 5 + (50 * valueIndex);
            Rect TimeValuePos = new Rect(positionX, position.y + HeightToLabels, 50, position.height);
            DrawTimeValueLabel(TimeValuePos, valueIndex);
        }

        private void DrawTimeSeparation(Rect position, int valueIndex) 
        {
            EditorGUIUtility.labelWidth = 100;
            float positionX = position.x + 40 + (50 * valueIndex);
            Rect newPos = new Rect(positionX, position.y, 10, position.height);
            EditorGUI.LabelField(newPos, new GUIContent(":"));
        }
        private void DrawTimeValueField(Rect position, int index, int maxValue) 
        {
            EditorGUIUtility.labelWidth = 0;
            int value = TimeValues.GetArrayElementAtIndex(index).intValue;
            value = EditorGUI.IntField(position, value);
            if (value > maxValue) value = maxValue;
            else if (value < 0) value = 0;
            TimeValues.GetArrayElementAtIndex(index).intValue = value;
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
        }
        #endregion
    }
}