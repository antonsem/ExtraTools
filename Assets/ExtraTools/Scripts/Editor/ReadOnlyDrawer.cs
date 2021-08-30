using UnityEditor;
using UnityEngine;

namespace ExtraTools
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ReadOnlyAttribute att = (ReadOnlyAttribute)attribute;
            object val = property.GetValue();

            if (att.warningIfNull && (val == null || val.ToString().Equals("null")))
                val += " <-This value should NOT be NULL!";

            EditorGUI.LabelField(position, string.Format("{0}: {1}", label.text, val));
        }
    }
}
