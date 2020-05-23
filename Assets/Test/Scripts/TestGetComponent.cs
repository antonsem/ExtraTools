using ExtraTools;
using UnityEngine;

public class TestGetComponent : MonoBehaviour
{
    public BoxCollider col;

    private void Start()
    {
        transform.EnsureComponent(out col, true, true);
    }
}
