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
		public readonly Color color;
		public readonly bool debugError;

		public RequiredField()
		{
			debugError = true;
			color = Color.red;
		}

		public RequiredField(FieldColor color)
		{
			debugError = true;
			this.color = color switch
			{
				FieldColor.Red => Color.red,
				FieldColor.Green => Color.green,
				FieldColor.Blue => Color.blue,
				FieldColor.Yellow => Color.yellow,
				_ => Color.red
			};
		}

		public RequiredField(Color color)
		{
			debugError = true;
			this.color = color;
		}

		public RequiredField(bool debugError = true, FieldColor color = FieldColor.Red)
		{
			this.debugError = debugError;
			this.color = color switch
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