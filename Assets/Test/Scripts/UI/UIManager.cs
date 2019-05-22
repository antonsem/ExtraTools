using ExtraTools;
using UnityEngine;

public enum Window
{
    None,
    MainMenu,
    InGame,
    Pause
}

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private InGame inGame;
    [SerializeField]
    private Pause pause;

    public void SetWindow(Window window)
    {
        mainMenu.gameObject.SetActive(window == Window.MainMenu);
        inGame.gameObject.SetActive(window == Window.InGame);
        pause.gameObject.SetActive(window == Window.Pause);
    }
}
