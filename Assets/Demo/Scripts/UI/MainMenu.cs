using UnityEngine;
using UnityEngine.UI;

namespace ExtraTools.Test
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private Button newGame;
		[SerializeField] private Button sendMessage;
		[SerializeField] private Button sendFiveMessages;
		[SerializeField] private Sprite messageSprite;

		private void Start()
		{
			newGame.onClick.AddListener(OnStartGame);
			sendMessage.onClick.AddListener(SendMessage);
			sendFiveMessages.onClick.AddListener(SendFiveMessages);
		}

		private static void OnStartGame()
		{
			UIManager.Instance.SetWindow(Window.InGame);
		}

		private void SendMessage()
		{
			MessageButton[] buttons =
			{
				new("Yes", () => Debug.Log("Yes")),
				new("No", () => Debug.Log("No")),
				new("Cancel", () => Debug.Log("Cancel"))
			};

			UIManager.Instance.PopUpMessage("This is a generic warning message. You can click on any button...",
				messageSprite, buttons);
		}

		private static void SendFiveMessages()
		{
			for (int i = 0; i < 5; i++)
			{
				UIManager.Instance.PopUpMessage($"message {i.ToString()}");
			}
		}
	}
}