using UnityEngine;
using System.IO;
using System.Reflection;
using UnityEditor;

namespace ExtraTools.Editor
{
	public static class EditorUtils
	{
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
			var assembly = Assembly.GetAssembly(typeof(SceneView));
			var type = assembly.GetType("UnityEditor.LogEntries");
			var method = type.GetMethod("Clear");
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
			var path = Directory.GetParent(Application.dataPath).FullName;
			EditorUtility.RevealInFinder(path);
		}

		public static object GetValue(this SerializedProperty property)
		{
			object obj = property.serializedObject.targetObject;
			var type = obj.GetType();
			var field = type.GetField(property.propertyPath,
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			return field?.GetValue(obj);
		}
	}
}