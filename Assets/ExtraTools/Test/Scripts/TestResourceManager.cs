using UnityEngine;

namespace ExtraTools.Test.Scripts
{
    public class TestResourceManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
                ResourceManager.Instance.testPrefab.Get();

            if (Input.GetKeyDown(KeyCode.U))
                ResourceManager.Instance.testPrefab.DestroyUnused();

            if (Input.GetKeyDown(KeyCode.A))
                ResourceManager.Instance.testPrefab.RemoveAll();

            if (Input.GetKeyDown(KeyCode.D))
                ResourceManager.Instance.testPrefab.DestroyAll();

            if (Input.GetKeyDown(KeyCode.P))
                ResourceManager.Instance.testPrefab.Prepare(10);
        }
    }
}
