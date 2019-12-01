using UnityEngine;

namespace ExtraTools
{

    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private bool _isQuitting = false;
        private static T _instance = null;
        public static T Instance
        {
            get
            {
                // Instance requiered for the first time, we look for it
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(T)) as T;

                    // Object not found, we create a temporary one
                    if (_instance == null)
                    {
                        Debug.LogWarning(string.Format("No instance of {0}, a temporary one is created.", typeof(T).ToString()));
                        _instance = new GameObject(string.Format("Temp Instance of {0}", typeof(T).ToString()), typeof(T)).GetComponent<T>();

                        // Problem during the creation, this should not happen
                        if (_instance == null)
                            Debug.LogError(string.Format("Problem during the creation of {0}", typeof(T).ToString()));
                    }
                }
                return _instance;
            }
        }
        // If no other monobehaviour request the instance in an awake function
        // executing before this one, no need to search the object.
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                _instance.Init();
            }
        }

        // This function is called when the instance is used the first time
        // Put all the initializations you need here, as you would do in Awake
        public virtual void Init() { }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        // Make sure the instance isn't referenced anymore when the object is destroyed
        protected virtual void OnDestroy()
        {
            if (!_isQuitting && _instance == this)
                _instance = null;
        }
    }
}
