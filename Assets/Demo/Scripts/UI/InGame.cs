using UnityEngine;
using UnityEngine.UI;

namespace ExtraTools.Test
{
	public class InGame : MonoBehaviour
	{
		[SerializeField] private Button pause;

		private void Start()
		{
			pause.onClick.AddListener(OnPause);
		}

		private static void OnPause()
		{
			UIManager.Instance.SetWindow(Window.Pause);
		}
	}
}