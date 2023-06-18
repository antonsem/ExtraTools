using UnityEngine;

namespace ExtraTools
{
	/// <summary>
	/// This is an empty MonoBehaviour. Its only purpose is to run coroutines
	/// </summary>
	public class CoroutineStarter : MonoBehaviour
	{
		private static CoroutineStarter _instance;

		public static CoroutineStarter Get
		{
			get
			{
				// Instance required for the first time, we look for it
				if (_instance) return _instance;

				_instance = FindObjectOfType<CoroutineStarter>();

				// Object not found, we create a temporary one
				if (_instance) return _instance;

				_instance = new GameObject($"Temp Instance of {typeof(CoroutineStarter)}", typeof(CoroutineStarter))
					.GetComponent<CoroutineStarter>();

				return _instance;
			}
		}
	}
}