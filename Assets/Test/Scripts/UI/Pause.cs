using UnityEngine;
using UnityEngine.UI;

namespace ExtraTools
{
    public class Pause : MonoBehaviour
    {
        [SerializeField]
        private Button resume;
        [SerializeField]
        private Button mainMenu;

        private void Start()
        {
            resume.onClick.AddListener(OnResume);
            mainMenu.onClick.AddListener(OnMainMenu);
        }

        private void OnResume()
        {
            UIManager.Instance.SetWindow(Window.InGame);
        }

        private void OnMainMenu()
        {
            UIManager.Instance.SetWindow(Window.MainMenu);
        }
    }
}
