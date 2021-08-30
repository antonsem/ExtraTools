using UnityEngine;

namespace ExtraTools
{
    public enum FieldColor
    {
        Red,
        Green,
        Blue,
        Yellow
    }

    /// <summary>
    /// Credit for this tip goes to Rodrigo Devora
    /// Got it from the twitter: https://twitter.com/Rodrigo_Devora/status/1204031607583264769
    /// </summary>
    public class RequiredField : PropertyAttribute
    {
        public Color color;
        public bool debugError = true;

        public RequiredField(FieldColor _color = FieldColor.Red)
        {
            color = _color switch
            {
                FieldColor.Red => Color.red,
                FieldColor.Green => Color.green,
                FieldColor.Blue => Color.blue,
                FieldColor.Yellow => Color.yellow,
                _ => Color.red
            };
        }
    }
}