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
        /// Executes TryGetComponent() on a transform, sets it to component, throws a warning if none is found
        /// or adds a component to the game object
        /// <typeparam name="T">Type of the component to search for</typeparam>
        /// <param name="t">Transform to search a component on</param>
        /// <param name="component">A component to be set</param>
        /// <param name="needWarning">Should a warning message be displayed if the component is not set</param>
        /// <param name="addedMessage">Should a message be displayed if the component is added</param>
        /// </summary>
        /// <returns>True if the component is set, false otherwise</returns>
        public static bool EnsureComponent<T>(this Transform t, out T component, bool needWarning = false, bool addedMessage = false) where T : Component
        {
            if (t.TryGetComponent(out component)) return component;
            if (needWarning)
                Debug.LogWarning($"{t.name} does not have a component of type {typeof(T)}!");

            component = t.gameObject.AddComponent<T>();
            if (addedMessage)
                Debug.Log($"Added a component of type {typeof(T)} to {t.name}");

            return component;
        }

#if UNITY_EDITOR

        /// <summary>
        /// Credit goes to FreCre (https://assetstore.unity.com/publishers/7617).
        /// Stumbled across this shortcut on twitter and decided to add
        /// to the Extra Tools. The original free asset package created by FreCre can be
        /// downloaded from the Asset Store: https://assetstore.unity.com/packages/tools/easyshortcutlockinspector-23579
        /// </summary>
        [MenuItem("Extra Tools/Toggle Inspector Lock %l")]
        private static void ToggleInspectorLock()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }

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
            method?.Invoke(new object(), null);
        }

        /// <summary>
        /// Credit goes to coeing from answers.unity.com
        /// Got this from here: https://answers.unity.com/questions/43422/how-to-implement-show-in-explorer.html
        /// PS: A similar function was added to the Shortcut editor in the 2019.1. Still, this
        /// can be used to open a specific folder (or an asset), not the one you clicked on in the Projects tab
        /// </summary>
        [MenuItem("Extra Tools/Open Project Folder %e")] // CTRL + E
        public static void ShowExplorer()
        {
            string path = Directory.GetParent(Application.dataPath).FullName;
            EditorUtility.RevealInFinder(path);
        }

        public static object GetValue(this SerializedProperty property)
        {
            object obj = property.serializedObject.targetObject;
            Type type = obj.GetType();
            FieldInfo field = type.GetField(property.propertyPath, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            return field?.GetValue(obj);
        }
#endif
    }
}