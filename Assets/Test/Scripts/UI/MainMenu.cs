using UnityEngine;
using UnityEngine.UI;

namespace ExtraTools
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Button newGame;
        [SerializeField]
        private Button sendMessage;
        [SerializeField]
        private Button sendFiveMessages;
        [SerializeField]
        private Sprite messageSprite;

        private void Start()
        {
            newGame.onClick.AddListener(OnStartGame);
            sendMessage.onClick.AddListener(SendMessage);
            sendFiveMessages.onClick.AddListener(SendFiveMessages);
        }

        private void OnStartGame()
        {
            UIManager.Instance.SetWindow(Window.InGame);
        }

        private void SendMessage()
        {
            UIManager.Instance.PopUpMessage("This is a generic warning message. You can click on any button...",
                null, () => Debug.Log("Yes"), () => Debug.Log("No"), () => Debug.Log("Cancel"), messageSprite);
        }

        private void SendFiveMessages()
        {
            for (int i = 0; i < 5; i++)
                UIManager.Instance.PopUpMessage("message " + i);
        }
    }
}
