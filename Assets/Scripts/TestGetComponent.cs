using ExtraTools;
using UnityEngine;

public class TestGetComponent : MonoBehaviour
{
    public BoxCollider col;

    void Start()
    {
        transform.GetComponent(out col, true, true, true);
    }
}
