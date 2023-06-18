using UnityEngine;

namespace ExtraTools
{
	public class StringDemo : MonoBehaviour
	{
		[Header("Empty string")]
		[SerializeField] private string testString;
		
		[Header("String formatting")]
		[SerializeField, ReadOnly] private string stringToFormat = "First parameter: {0}, second parameter: {1}";
		[Space, SerializeField] private string[] parameters;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				Debug.Log($"testString is not null or empty: {testString.IsValid()}");
			}

			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				Debug.Log($"Formatted string: {stringToFormat.Format(parameters)}");
			}
		}
	}
}