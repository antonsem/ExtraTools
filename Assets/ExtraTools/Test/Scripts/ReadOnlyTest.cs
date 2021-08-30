using UnityEngine;

namespace ExtraTools.Test.Scripts
{
    public class ReadOnlyTest : MonoBehaviour
    {
        #pragma warning disable 0414
        [ReadOnly(true)] public Rigidbody rigid;
        [SerializeField, ReadOnly] private float testFloatValue = 123;
        #pragma warning restore 0414
    }
}
