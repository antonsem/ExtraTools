using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    [SerializeField]
    private Button pause;

    private void Start()
    {
        pause.onClick.AddListener(OnPause);
    }

    private void OnPause()
    {
        UIManager.Instance.SetWindow(Window.Pause);
    }
}
