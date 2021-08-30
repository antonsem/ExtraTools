using UnityEngine;

namespace ExtraTools.Test.Scripts
{
    public class HighlightTest : MonoBehaviour
    {
        [RequiredField] public GameObject shouldBeAssigned;
        [RequiredField(FieldColor.Yellow)] public GameObject yellowField;
        [SerializeField, RequiredField(FieldColor.Blue)] private GameObject serializedHighlight;
    }
}
