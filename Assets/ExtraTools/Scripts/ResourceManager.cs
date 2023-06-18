using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ExtraTools
{
	public class ResourceManager : Singleton<ResourceManager>
	{
		private interface IValidate
		{
			bool OnValidate();
		}

		[Serializable]
		public class ObjectHolder<T> : IValidate where T : Component
		{
			[Tooltip("The object to instantiate"), RequiredField, SerializeField]
			private GameObject prefab;
			[Tooltip("The pool of instantiated objects"), SerializeField]
			private List<T> componentPool = new List<T>();

			/// <summary>
			/// Returns a game object's component from the pool. Instantiate a new one if none is available
			/// </summary>
			/// <param name="position">Position of the object</param>
			/// <param name="rotation">Rotation of the object</param>
			/// <param name="parent">Parent of the new object</param>
			public T Get(Vector3 position, Vector3 rotation, Transform parent)
			{
				for (var i = 0; i < componentPool.Count; i++)
				{
					if (componentPool[i].gameObject.activeInHierarchy)
						continue;

					componentPool[i].transform.SetParent(parent);
					componentPool[i].transform.position = position;
					componentPool[i].transform.rotation = Quaternion.Euler(rotation);
					componentPool[i].gameObject.SetActive(true);
					return componentPool[i];
				}

				var obj = Instantiate(prefab, position, Quaternion.Euler(rotation));

				if (!obj.transform.TryGetComponent(out T component))
				{
					Debug.LogError($"Prefab {prefab.name} doesn't have a component of type {typeof(T)}!");
					return null;
				}

				componentPool.Add(component);
				componentPool[componentPool.Count - 1].transform.SetParent(parent);
				return componentPool[componentPool.Count - 1];
			}

			/// <summary>
			/// Returns a game object's component from the pool. Instantiate a new one if none is available
			/// </summary>
			/// <param name="parent">Parent of the new object</param>
			public T Get(Transform parent = null)
			{
				return Get(default, default, parent);
			}

			/// <summary>
			/// Returns a game object's component from the pool. Instantiate a new one if none is available
			/// </summary>
			/// <param name="position">Position of the object</param>
			/// <param name="parent">Parent of the new object</param>
			public T Get(Vector3 position, Transform parent = null)
			{
				return Get(position, default, parent);
			}

			/// <summary>
			/// Destroys currently disabled objects
			/// </summary>
			public void DestroyUnused()
			{
				for (var i = componentPool.Count - 1; i >= 0; i--)
				{
					if (componentPool[i].gameObject.activeInHierarchy)
						continue;

					Destroy(componentPool[i]);
					componentPool.Remove(componentPool[i]);
				}
			}

			/// <summary>
			/// Disables all objects and moves them under the pool
			/// </summary>
			public void RemoveAll()
			{
				for (var i = 0; i < componentPool.Count; i++)
					Remove(componentPool[i].gameObject);
			}

			/// <summary>
			/// Destroys all objects
			/// </summary>
			public void DestroyAll()
			{
				for (var i = componentPool.Count - 1; i >= 0; i--)
					Destroy(componentPool[i]);

				componentPool.Clear();
			}

			/// <summary>
			/// Instantiates new objects to the pool
			/// </summary>
			/// <param name="count">Number of objects to prepare</param>
			public void Prepare(int count = 1)
			{
				for (var i = 0; i < count; i++)
				{
					var
						obj = Instantiate(prefab, Pool); //Instantiate under the Pool so the Awake() is not invoked

					if (!obj.transform.TryGetComponent(out T component))
					{
						Debug.LogError($"Prefab {prefab.name} doesn't have a component of type {typeof(T)}!");
						continue;
					}

					componentPool.Add(component);
					componentPool[componentPool.Count - 1].gameObject.SetActive(false);
				}
			}

			public bool OnValidate()
			{
				if (!prefab)
				{
					Debug.LogError($"A prefab with a component of type {typeof(T)} is not set on the ResourceManager!",
						Instance);
					return false;
				}

				if (prefab.GetComponent<T>())
					return true;

				Debug.LogError($"Prefab {prefab} does not have a component of type {typeof(T)}!", prefab);
				prefab = null;
				return false;
			}
		}

		//A transform to store inactive objects
		private static Transform pool;
		public static Transform Pool
		{
			get
			{
				if (pool) return pool;
				pool = new GameObject("Pool").transform;
				pool.gameObject.SetActive(false);

				return pool;
			}
		}

		/// <summary>
		/// Disables the object and moves it under the Pool object
		/// </summary>
		/// <param name="obj">Object to disable</param>
		public static void Remove(GameObject obj)
		{
			obj.SetActive(false);
			obj.transform.SetParent(Pool);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			var type = GetType();
			var fields =
				type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

			for (var index = 0; index < fields.Length; ++index)
			{
				if (!(fields[index].GetValue(this) is IValidate obj) || obj.OnValidate())
					continue;

				Debug.LogError(
					$"{fields[index].Name} is not valid! Set the a prefab with a component of type {fields[index].FieldType}!",
					this);
			}
		}
#endif

		public ObjectHolder<Transform> testPrefab;
	}
}