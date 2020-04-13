using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using QNA;

namespace CustomEditors.QNA
{
    [CustomEditor(typeof(Evaluator))]
    public class EvaluatorEditor : Editor
    {
        #region EDITOR METHODS
        private void OnEnable()
        {
            SetEditorVariables();
            evaluator.CreateEditorVariables();

            ButtonSettingsEditor.OnEnable(ButtonSettings);
            UIEditor.OnEnable(UI);
        }

        public override void OnInspectorGUI()
        {
            EditorGUIUtility.labelWidth = EditorVariables.MediumLabelWidth;
            serializedObject.Update();
            SelectedTab.intValue = GUILayout.Toolbar(SelectedTab.intValue, ToolbarTabs);
            GUILayout.Space(6);
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
            GUILayout.Space(6);
            switch (SelectedTab.intValue) 
            {
                case 0: DrawGeneralVariables(); break;
                case 1: UIEditor.OnInspectorGUI(); break;
                case 2: ButtonSettingsEditor.OnInspectorGUI();break;
                case 3: OpenCreateInterface(); break;
                default:return;
            }
            GUILayout.Space(10);

            serializedObject.ApplyModifiedProperties();

            //base.OnInspectorGUI();
        }
        #endregion

        #region VARIABLES
        private EvaluationUIEditor UIEditor;
        private SerializedProperty UI;
        private ButtonSettingsEditor ButtonSettingsEditor;
        private SerializedProperty ButtonSettings;
        private Evaluator evaluator;

        private SerializedProperty SelectedTab;
        private SerializedProperty highlight_duration;
        private SerializedProperty OnFinishedEvaluationAction;

        private string[] ToolbarTabs;
        #endregion

        #region PRIVATE METHODS
        private void OpenCreateInterface() 
        {
            if (GUILayout.Button(new GUIContent("Open Creator Editor"))) 
            {
                TestCreatorWindow.DisplayWindow(serializedObject);
            }
        }

        private void SetEditorVariables()
        {
            evaluator = (Evaluator)target;
            highlight_duration = serializedObject.FindProperty("highlight_duration");
            OnFinishedEvaluationAction = serializedObject.FindProperty("OnFinishedEvaluationAction");
            SelectedTab = serializedObject.FindProperty("SelectedTab");

            ButtonSettings = serializedObject.FindProperty("ButtonSettings");
            UI = serializedObject.FindProperty("UI");

            ButtonSettingsEditor = new ButtonSettingsEditor();
            UIEditor = new EvaluationUIEditor();

            ToolbarTabs = new string[4] { "General", "Interfaces", "Buttons", "Create" };
        }

        private void DrawGeneralVariables() 
        {
            EditorGUILayout.PropertyField(
                highlight_duration,
                new GUIContent("Highlight Time", "The duration in seconds of the time the answer options will remain highlightened after the question has been answered."),
                GUILayout.MaxWidth(EditorVariables.LargePropertyidth));

            GUILayout.Space(5);

            EditorGUILayout.PropertyField(
                OnFinishedEvaluationAction,
                new GUIContent("After Actions", "The actions to be called after the completition of the test."),
                GUILayout.MaxWidth(EditorVariables.LargePropertyidth));
        }
        #endregion
    }
}