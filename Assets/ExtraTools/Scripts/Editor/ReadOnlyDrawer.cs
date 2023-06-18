using UnityEditor;
using UnityEngine;

namespace ExtraTools.Editor
{
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyDrawer : PropertyDrawer
	{
		private const string NULL_ERROR =
			"Read Only field <color=red>{0}</color> is not assigned on <color=blue>{1}</color>!";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var att = attribute as ReadOnlyAttribute;

			if (att.errorIfNull && property.GetValue().Equals(null))
			{
				Debug.LogErrorFormat(property.serializedObject.targetObject, NULL_ERROR, label.text,
					property.serializedObject.targetObject);
			}

			var previousGUIState = GUI.enabled;
			GUI.enabled = false;
			EditorGUI.PropertyField(position, property, label);
			GUI.enabled = previousGUIState;
		}
	}
}