using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button newGame;

    private void Start()
    {
        newGame.onClick.AddListener(OnStartGame);
    }

    private void OnStartGame()
    {
        UIManager.Instance.SetWindow(Window.InGame);
    }
}
