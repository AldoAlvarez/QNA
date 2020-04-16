using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestCreatorWindow : EditorWindow
{
    public static void DisplayWindow(SerializedObject obj)
    {
 
        TestCreatorWindow window = (TestCreatorWindow)EditorWindow.GetWindow(typeof(TestCreatorWindow));
        window.Show();
        window.minSize = new Vector2(150, 200);
        if (obj != myObject) 
        {
            myObject = obj;
            createdvariables = false;
        }
    }

    private void OnGUI()
    {
        if (myObject == null) 
        {
            TestCreatorWindow window = (TestCreatorWindow)EditorWindow.GetWindow(typeof(TestCreatorWindow));
            window.Close();
            return;
        }
        SetEditorVariables();
        if (!createdvariables) return;

        EditorGUILayout.PropertyField(currentTest);
        EditorGUILayout.PropertyField(currentQuestion);

        myObject.ApplyModifiedProperties();
    }

    #region VARIABLES
    private QNA.TestCreator creator;

    private static SerializedObject myObject;
    private SerializedProperty testCreator;
    private SerializedProperty currentTest;
    private SerializedProperty currentQuestion;
    private SerializedProperty TestsContainer;

    private static bool createdvariables = false;
    #endregion

    #region PRIVATE METHODS
    private void SetEditorVariables() 
    {
        if (createdvariables) return;
        testCreator = myObject.FindProperty("testCreator");
        creator = ((QNA.Evaluator)myObject.targetObject).testCreator;
        if (testCreator == null) return;
        currentTest = testCreator.FindPropertyRelative("currentTest");
        currentQuestion = testCreator.FindPropertyRelative("currentQuestion");
        TestsContainer = testCreator.FindPropertyRelative("TestsContainer");
        createdvariables = true;
    }
    #endregion
}
