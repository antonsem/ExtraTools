using UnityEngine;

namespace ExtraTools.Test.Scripts
{
	public class DemoResource : MonoBehaviour
	{
		private void Awake()
		{
			Debug.Log($"Awake invoked on {this}", this);
		}

		private void Start()
		{
			Debug.Log($"Start invoked on {this}", this);
		}

		private void OnEnable()
		{
			Debug.Log($"OnEnable invoked on {this}", this);
		}
	}
}