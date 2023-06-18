using UnityEngine;

namespace ExtraTools.Test
{
	public enum Window
	{
		None,
		MainMenu,
		InGame,
		Pause
	}

	public class UIManager : Singleton<UIManager>
	{
		[SerializeField] private MainMenu mainMenu;
		[SerializeField] private InGame inGame;
		[SerializeField] private Pause pause;
		[SerializeField] private PopUp popUp;

		/// <summary>
		/// Set a specific UI window
		/// </summary>
		/// <param name="window">Type of the window to be set</param>
		public void SetWindow(Window window)
		{
			mainMenu.gameObject.SetActive(window == Window.MainMenu);
			inGame.gameObject.SetActive(window == Window.InGame);
			pause.gameObject.SetActive(window == Window.Pause);
		}

		public void PopUpMessage(string msg, Sprite img = null, params MessageButton[] buttons)
		{
			popUp.Register(msg, img, buttons);
		}
	}
}