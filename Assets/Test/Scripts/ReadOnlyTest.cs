using UnityEngine;

namespace ExtraTools
{
    public class ReadOnlyTest : MonoBehaviour
    {
        [ReadOnly(true)]
        public Rigidbody rigid;

        [SerializeField, ReadOnly]
        private float testFloatValue = 123;
    }
}
