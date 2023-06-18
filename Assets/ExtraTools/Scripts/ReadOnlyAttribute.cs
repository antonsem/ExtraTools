using UnityEngine;

namespace ExtraTools
{
	public class ReadOnlyAttribute : PropertyAttribute
	{
		/// <summary>
		/// Writes a warning if the value of this field is null
		/// </summary>
		public readonly bool errorIfNull;

		public ReadOnlyAttribute()
		{
			errorIfNull = true;
		}

		public ReadOnlyAttribute(bool errorIfNull)
		{
			this.errorIfNull = errorIfNull;
		}
	}
}