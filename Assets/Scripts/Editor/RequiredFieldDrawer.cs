using UnityEditor;
using UnityEngine;

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
            
            if (property.objectReferenceValue == null)
            {
                if (field.debugError)
                    Debug.LogErrorFormat(property.serializedObject.targetObject, "Required field <color=red>{0}</color> is not assigned on <color=blue>{1}</color>!", label.text, property.serializedObject.targetObject);

                GUI.color = field.color;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = Color.white;
            }
            else
                EditorGUI.PropertyField(position, property, label);
        }
    }
}