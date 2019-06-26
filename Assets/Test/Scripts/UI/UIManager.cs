using UnityEngine;
using UnityEngine.Events;

namespace ExtraTools
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
        [SerializeField]
        private MainMenu mainMenu;
        [SerializeField]
        private InGame inGame;
        [SerializeField]
        private Pause pause;
        [SerializeField]
        private PopUp popUp;

        [SerializeField, ReadOnly]
        protected float asd = 213;

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

        public void PopUpMessage(string msg, UnityAction ok = null, UnityAction yes = null, UnityAction no = null, UnityAction cancel = null, Sprite img = null)
        {
            popUp.Register(msg, ok, yes, no, cancel, img);
        }
    }
}
