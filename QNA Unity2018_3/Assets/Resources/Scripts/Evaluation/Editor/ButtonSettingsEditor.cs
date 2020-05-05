using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.Evaluation.ButtonLabel;
using AGAC.Evaluation.Base;

namespace CustomEditors.Evaluation {

    public class ButtonSettingsEditor
    {
        private SerializedProperty LabelColorSettings;
        private SerializedProperty Colors;
        private SerializedProperty foldutSettings;

        private GUIContent foldoutLabelText;
        private SerializedProperty lastSelectedIndex;

        public void OnEnable(SerializedProperty AnswerButtonSettings) 
        {
            LabelColorSettings = AnswerButtonSettings.FindPropertyRelative("LabelColorSettings");
            lastSelectedIndex = AnswerButtonSettings.FindPropertyRelative("lastSelectedIndex");
            foldutSettings = AnswerButtonSettings.FindPropertyRelative("editorFoldoutPerSetting");
        }
        public void OnInspectorGUI ()
        {
            for (int colorSetting = 0; colorSetting < foldutSettings.arraySize; ++colorSetting) 
            {
                GUIStyle boldFoldout = EditorStyles.foldout;
                boldFoldout.fontStyle = FontStyle.Bold;

                foldoutLabelText = new GUIContent(((Settings)colorSetting).ToString());
                bool foldoutSettingValue = EditorGUILayout.Foldout(SettingIsExpanded(colorSetting), foldoutLabelText, true, boldFoldout);
                
                boldFoldout.fontStyle = FontStyle.Normal;

                if (foldoutSettingValue) 
                {
                    if (colorSetting != lastSelectedIndex.intValue)
                    {
                        InvertFoldoutValues(colorSetting);
                        lastSelectedIndex.intValue = colorSetting;
                        Colors = LabelColorSettings.GetArrayElementAtIndex(colorSetting).FindPropertyRelative("Colors");
                    }
                    GUILayout.Space(3);
                    DrawColorSetting(colorSetting);
                }
                GUILayout.Space(4);
            }
        }

        private bool SettingIsExpanded(int settingIndex) 
        {
            return foldutSettings.GetArrayElementAtIndex(settingIndex).boolValue;
        }

        private void InvertFoldoutValues(int newIndex)
        {
            if (lastSelectedIndex.intValue >= 0)
            {
                foldutSettings.GetArrayElementAtIndex(lastSelectedIndex.intValue).boolValue = false;
                foldutSettings.GetArrayElementAtIndex(newIndex).boolValue = true;
            }
            else 
            {
                foldutSettings.GetArrayElementAtIndex(newIndex).boolValue = true;
            }
        }

        private void DrawColorSetting(int settingIndex) 
        {
            if (Colors == null)
                Colors = LabelColorSettings.GetArrayElementAtIndex(settingIndex).FindPropertyRelative("Colors");

            for (int color = 0; color < Colors.arraySize; ++color) 
            {
                GUIContent label = new GUIContent(AnswerButtonSettings.GetColorSettingName((Settings)settingIndex, color));
                EditorGUILayout.PropertyField(Colors.GetArrayElementAtIndex(color),label, GUILayout.MaxWidth(EditorVariables.LargePropertyidth));
            }
        }
    }
}