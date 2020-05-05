using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Drawertest))]
public class DrawerTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty question = serializedObject.FindProperty("question");
        EditorGUILayout.PropertyField(question);
        serializedObject.ApplyModifiedProperties();
    }
}