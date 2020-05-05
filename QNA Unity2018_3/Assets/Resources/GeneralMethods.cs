using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAC.General
{
    public static class GeneralMethods 
    {
        #region PUBLIC METHODS
        public static void CloseApp()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public static void AddElementToArray<T>(ref T[] array) where T : class
        {
            T[] new_array = new T[array.Length + 1];
            array.CopyTo(new_array, 0);

            array = new T[new_array.Length];
            new_array.CopyTo(array, 0);
        }

        public static void RemoveElementFromArray<T>(int index, ref T[] array)
        {
            if (array == null) return;
            if (index < 0 || index >= array.Length) return;

            T[] new_array = new T[array.Length - 1];

            for (int i = 0; i < index; ++i)
                new_array[i] = array[i];
            for (int i = index + 1; i < array.Length; ++i)
                new_array[i - 1] = array[i];

            array = new T[new_array.Length];
            new_array.CopyTo(array, 0);
        }

        public static T[] GetComponentsInParent<T>(Transform parentContainer) where T:Component
        {
            List<T>Components = new List<T>();
            for (int i = 0; i < parentContainer.childCount; ++i) 
            {
                T child_component = parentContainer.GetChild(i).GetComponent<T>();
                if (child_component != null) 
                    Components.Add(child_component);
            }
            return Components.ToArray();
        }

        public static T GetInstance<T>(string objectName) where T : Behaviour
        {
            T[] sceneObjects = GameObject.FindObjectsOfType<T>();
            if (sceneObjects == null || sceneObjects.Length <= 0)
                return GetNewObjectInstance<T>(objectName);
            else
                return GetActiveObjectInstance<T>(sceneObjects);
        }

        public static GUIStyle CreateBasicGUIStyle(GUIStyle base_style, Color background, Color text)
        {
            GUIStyle style = new GUIStyle(base_style);
            style.normal.background = GetTexture(background);
            style.normal.textColor = text;
            return style;
        }
        public static Texture2D GetTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
        #endregion

        #region PRIVATE METHODS
        private static T GetNewObjectInstance<T>(string name) where T: Behaviour
        {
            return new GameObject(name).AddComponent<T>();
        }
        private static T GetActiveObjectInstance<T>(T[]objects) where T : Behaviour 
        {
            T activeObject = null;
            for (int obj = 0; obj < objects.Length; ++obj) 
            {
                if (activeObject != null)
                    objects[obj].enabled = false;
                else if (objects[obj].enabled)
                    activeObject = objects[obj];
            }

            if (activeObject == null) 
            {
                objects[0].enabled = true;
                activeObject = objects[0];
            }
            return activeObject;
        }
        #endregion
    }
}