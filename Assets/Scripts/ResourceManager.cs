using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtraTools
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        [Serializable]
        public class ObjectHolder
        {
            [Tooltip("The object to instantiate"), SerializeField]
            private GameObject prefab;
            [Tooltip("The pool of instantated objects"), SerializeField]
            private List<GameObject> pool = new List<GameObject>();

            /// <summary>
            /// Returns a game object from the pool. Instantiate a new one if none is available
            /// </summary>
            /// <param name="position">Position of the object</param>
            /// <param name="rotation">Rotation of the object</param>
            public GameObject Get(Vector3 position = default(Vector3), Vector3 rotation = default(Vector3), Transform parent = null)
            {
                for (int i = 0; i < pool.Count; i++)
                {
                    if (!pool[i].activeInHierarchy)
                    {
                        pool[i].transform.SetParent(parent);
                        pool[i].transform.position = position;
                        pool[i].transform.rotation = Quaternion.Euler(rotation);
                        pool[i].SetActive(true);
                        return pool[i];
                    }
                }

                pool.Add(Instantiate(prefab, position, Quaternion.Euler(rotation)));
                pool[pool.Count - 1].transform.SetParent(parent);
                return pool[pool.Count - 1];
            }

            /// <summary>
            /// Destroyes currently disabled objects
            /// </summary>
            public void DestroyUnused()
            {
                for (int i = 0; i < pool.Count; i++)
                {
                    if (!pool[i].activeInHierarchy)
                    {
                        Destroy(pool[i]);
                        pool.Remove(pool[i]);
                    }
                }
            }

            /// <summary>
            /// Disables all objects and moves them under the pool
            /// </summary>
            public void RemoveAll()
            {
                for (int i = 0; i < pool.Count; i++)
                    ResourceManager.Remove(pool[i]);
            }

            /// <summary>
            /// Destroyes all objects
            /// </summary>
            public void DestroyAll()
            {
                for (int i = 0; i < pool.Count; i++)
                    Destroy(pool[i]);

                pool.Clear();
            }

            /// <summary>
            /// Instantiates new objects to the pool
            /// </summary>
            /// <param name="count">Number of objects to prepare</param>
            public void Prepare(int count = 1)
            {
                for (int i = 0; i < count; i++)
                {
                    pool.Add(Instantiate(prefab, Pool));//Instantiate under the Pool so the Awake() is not invoked
                    pool[pool.Count - 1].SetActive(false);
                }
            }
        }

        //A transform to store inactive objects
        private static Transform pool;
        public static Transform Pool
        {
            get
            {
                if (!pool)
                {
                    pool = new GameObject("Pool").transform;
                    pool.gameObject.SetActive(false);
                }

                return pool;
            }
        }

        public ObjectHolder testPrefab;

        /// <summary>
        /// Disables the object and moves it under the Pool objecg
        /// </summary>
        /// <param name="obj">Object to disable</param>
        public static void Remove(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(Pool);
        }
    }
}
