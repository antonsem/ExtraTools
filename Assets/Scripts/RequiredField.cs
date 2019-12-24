using System.Collections;
using System.Collections.Generic;
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

        public RequiredField(FieldColor _color = FieldColor.Red)
        {
            switch (_color)
            {
                case FieldColor.Red:
                    color = Color.red;
                    break;
                case FieldColor.Green:
                    color = Color.green;
                    break;
                case FieldColor.Blue:
                    color = Color.blue;
                    break;
                case FieldColor.Yellow:
                    color = Color.yellow;
                    break;
                default:
                    color = Color.red;
                    break;
            }
        }
    }
}
