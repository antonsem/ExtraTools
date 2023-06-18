using UnityEngine;

namespace ExtraTools.Test.Scripts
{
	public class DemoGetComponent : MonoBehaviour
	{
		public BoxCollider col;

		private void Start()
		{
			transform.EnsureComponent(out col, true, true);
		}
	}
}