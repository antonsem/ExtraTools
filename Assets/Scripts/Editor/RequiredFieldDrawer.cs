using UnityEngine;
using UnityEditor;

namespace ExtraTools
{
    /// <summary>
    /// Credit for this tip goes to Rodrigo Devora
    /// Got it from the twitter: https://twitter.com/Rodrigo_Devora/status/1204031607583264769
    /// </summary>
    [CustomPropertyDrawer(typeof(RequiredField))]
    public class RequiredFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RequiredField field = attribute as RequiredField;

            if(property.objectReferenceValue == null)
            {
                GUI.color = field.color;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = Color.white;
            }
            else
                EditorGUI.PropertyField(position, property, label);

        }
    }
}
