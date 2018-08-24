#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
#endif

//I will be adding some useful stuff I use in my projects.
namespace ExtraTools
{
    public static class Utils
    {
#if UNITY_EDITOR
        /// <summary>
        /// Credit goes to KirillKuzyk from answers.unity.com.
        /// Got this from the here: https://answers.unity.com/questions/707636/clear-console-window.html
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
#endif
    }
}