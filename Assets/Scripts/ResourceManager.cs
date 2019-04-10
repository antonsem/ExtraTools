using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtraTools
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        [Serializable]
        private class ObjectHolder
        {
            [SerializeField]
            private GameObject prefab;
            [SerializeField]
            private List<GameObject> instantiated = new List<GameObject>();
        }
    }
}
