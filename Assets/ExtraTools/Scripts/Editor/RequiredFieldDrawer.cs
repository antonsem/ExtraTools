using UnityEditor;
using UnityEngine;

namespace ExtraTools.Editor
{
	/// <summary>
	/// Credit for this tip goes to Rodrigo Devora
	/// Got it from the twitter: https://twitter.com/Rodrigo_Devora/status/1204031607583264769
	/// </summary>
	[CustomPropertyDrawer(typeof(RequiredField))]
	public class RequiredFieldDrawer : PropertyDrawer
	{
		private const string ERROR =
			"Required field <color=red>{0}</color> is not assigned on <color=blue>{1}</color>!";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var field = attribute as RequiredField;

			if (!property.GetValue().Equals(null))
			{
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			if (field.debugError)
			{
				Debug.LogErrorFormat(property.serializedObject.targetObject, ERROR, label.text,
					property.serializedObject.targetObject);
			}

			var previousGUIColor = GUI.color;
			GUI.color = field.color;
			EditorGUI.PropertyField(position, property, label);
			GUI.color = previousGUIColor;
		}
	}
}