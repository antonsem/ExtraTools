#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
#endif
using UnityEngine;

//I will be adding some useful stuff I use in my projects.
namespace ExtraTools
{
    public static class Utils
    {

        /// <summary>
        /// Executes GetComponent<T>() on a transform, sets it to component,thows a warining if none is found
        /// or adds a component to the game object
        /// </summary>
        /// <typeparam name="T">Type of the component to search for</typeparam>
        /// <param name="t">Tranform to search a component on</param>
        /// <param name="component">A component to be set</param>
        /// <param name="needWarning">Should a warning message be displayed if the component is not set</param>
        /// <param name="ensureComponent">Shold the component be added if none is found</param>
        /// <param name="addedMessage">Should a message be displayed if the component is added</param>
        /// <returns>True if the component is set, false otherwise</returns>
        public static bool GetComponent<T>(this Transform t, out T component, bool needWarning = false, bool ensureComponent = false, bool addedMessage = false) where T : Component
        {
            component = t.GetComponent<T>();
            if (!component)
            {
                if (needWarning)
                    Debug.LogWarning(string.Format("{0} does not have a component of type {1}!", t.name, typeof(T)));

                if (ensureComponent)
                {
                    component = t.gameObject.AddComponent<T>();
                    if (addedMessage)
                        Debug.Log(string.Format("Added a component of type {0} to {1}", typeof(T), t.name));
                }

            }

            return component;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Credit goes to KirillKuzyk from answers.unity.com.
        /// Got this from here: https://answers.unity.com/questions/707636/clear-console-window.html
        /// For more information about MenuItem see documentation: https://docs.unity3d.com/ScriptReference/MenuItem.html
        /// </summary>
        [MenuItem("Extra Tools/Clear Console %q")] // CTRL + Q
        private static void ClearConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        /// <summary>
        /// Credit goes to coeing from answers.unity.com
        /// Got this from here: https://answers.unity.com/questions/43422/how-to-implement-show-in-explorer.html
        /// </summary>
        [MenuItem("Extra Tools/Open Project Folder %e")] // CTRL + E
        public static void ShowExplorer()
        {
            string path = Directory.GetParent(Application.dataPath).FullName;
            EditorUtility.RevealInFinder(path);
        }
#endif
    }
}