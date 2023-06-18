using UnityEngine;
using UnityEngine.UI;

namespace ExtraTools.Test
{
	public class Pause : MonoBehaviour
	{
		[SerializeField] private Button resume;
		[SerializeField] private Button mainMenu;

		private void Start()
		{
			resume.onClick.AddListener(OnResume);
			mainMenu.onClick.AddListener(OnMainMenu);
		}

		private static void OnResume()
		{
			UIManager.Instance.SetWindow(Window.InGame);
		}

		private static void OnMainMenu()
		{
			UIManager.Instance.SetWindow(Window.MainMenu);
		}
	}
}