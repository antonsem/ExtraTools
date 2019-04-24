using UnityEngine;

public class TestResource : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake invoked on " + this);
    }

    private void Start()
    {
        Debug.Log("Start invoked on " + this);
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable invoked on " + this);
    }
}
