using System.Collections.Generic;
using UnityEngine;

namespace ExtraTools.Test.Scripts
{
    public class TestRandomListItem : MonoBehaviour
    {
        [SerializeField] private string[] strings;
        [SerializeField] private List<float> floats;
        [SerializeField] private List<Transform> transforms;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
                Debug.Log(strings.GetRandom());
            if(Input.GetKeyDown(KeyCode.Alpha2))
                Debug.Log(floats.GetRandom());
            if(Input.GetKeyDown(KeyCode.Alpha3))
                Debug.Log(transforms.GetRandom());
        }
    }
}