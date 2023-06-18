using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//I will be adding some useful stuff I use in my projects.
namespace ExtraTools
{
	public static class Utils
	{
		/// <summary>
		/// Executes TryGetComponent() on a transform, sets it to component, throws a warning if none is found
		/// or adds a component to the game object
		/// <typeparam name="T">Type of the component to search for</typeparam>
		/// <param name="transform">Transform to search a component on</param>
		/// <param name="component">A component to be set</param>
		/// <param name="needWarning">Should a warning message be displayed if the component is not set</param>
		/// <param name="addedMessage">Should a message be displayed if the component is added</param>
		/// </summary>
		/// <returns>True if the component is set, false otherwise</returns>
		public static void EnsureComponent<T>(this Transform transform, out T component, bool needWarning = false,
			bool addedMessage = false) where T : Component
		{
			if (transform.TryGetComponent(out component))
			{
				return;
			}

			if (needWarning)
			{
				Debug.LogWarning($"{transform.name} does not have a component of type {typeof(T)}!");
			}

			component = transform.gameObject.AddComponent<T>();

			if (addedMessage)
			{
				Debug.Log($"Added a component of type {typeof(T)} to {transform.name}");
			}
		}

		/// <summary>
		/// Checks if the string is null or empty
		/// </summary>
		/// <returns>True if the string is not null or empty</returns>
		public static bool IsValid(this string str)
		{
			return !string.IsNullOrEmpty(str);
		}

		/// <summary>
		/// A shortcut for string.Format(string, params object[])
		/// </summary>
		/// <param name="str">String to format</param>
		/// <param name="parameters">Parameters to insert into the string</param>
		/// <returns>string.Format(str, parameters)</returns>
		public static string Format(this string str, params object[] parameters)
		{
			return string.Format(str, parameters);
		}

		/// <summary>
		/// Gets a random item from a list or an array
		/// </summary>
		/// <returns>Random item if any items exist. Default value of the type otherwise.</returns>
		public static T GetRandom<T>(this IReadOnlyList<T> list)
		{
			if (list != null && list.Count != 0)
			{
				return list[Random.Range(0, list.Count)];
			}

			Debug.LogError("List doesn't have any items. Returning default");
			return default;
		}

		#region Material Property Block

		private static MaterialPropertyBlock _materialProperty;

		private static MaterialPropertyBlock MaterialPropertyBlock => _materialProperty ??= new MaterialPropertyBlock();

		/// <summary>
		/// Sets a property of a material using a PropertyBlock (i.e. without create new material instances)
		/// </summary>
		/// <param name="renderer">Renderer to set the property of</param>
		/// <param name="propertyHash">Hash of the property</param>
		/// <param name="value">New value of the property</param>
		/// <param name="materialIndex">Material index on the renderer to set the property of</param>
		public static void SetProperty(this Renderer renderer, int propertyHash, int value, int materialIndex = 0)
		{
			if (materialIndex >= renderer.materials.Length)
			{
				Debug.LogWarning(
					$"Renderer {renderer.name} has {renderer.materials.Length.ToString()} materials. You are trying to access {materialIndex}. material.");
				return;
			}

			renderer.GetPropertyBlock(MaterialPropertyBlock, materialIndex);

			MaterialPropertyBlock.SetInt(propertyHash, value);

			renderer.SetPropertyBlock(MaterialPropertyBlock);
		}

		/// <summary>
		/// Sets a property of a material using a PropertyBlock (i.e. without create new material instances)
		/// </summary>
		/// <param name="renderer">Renderer to set the property of</param>
		/// <param name="propertyName">Name of the property</param>
		/// <param name="value">New value of the property</param>
		/// <param name="materialIndex">Material index on the renderer to set the property of</param>
		public static void SetProperty(this Renderer renderer, string propertyName, int value, int materialIndex = 0)
		{
			renderer.SetProperty(Shader.PropertyToID(propertyName), value, materialIndex);
		}

		/// <summary>
		/// Sets a property of a material using a PropertyBlock (i.e. without create new material instances)
		/// </summary>
		/// <param name="renderer">Renderer to set the property of</param>
		/// <param name="propertyHash">Hash of the property</param>
		/// <param name="value">New value of the property</param>
		/// <param name="materialIndex">Material index on the renderer to set the property of</param>
		public static void SetProperty(this Renderer renderer, int propertyHash, float value, int materialIndex = 0)
		{
			if (materialIndex >= renderer.materials.Length)
			{
				Debug.LogWarning(
					$"Renderer {renderer.name} has {renderer.materials.Length.ToString()} materials. You are trying to access {materialIndex}. material.");
				return;
			}

			renderer.GetPropertyBlock(MaterialPropertyBlock, materialIndex);

			MaterialPropertyBlock.SetFloat(propertyHash, value);

			renderer.SetPropertyBlock(MaterialPropertyBlock);
		}

		/// <summary>
		/// Sets a property of a material using a PropertyBlock (i.e. without create new material instances)
		/// </summary>
		/// <param name="renderer">Renderer to set the property of</param>
		/// <param name="propertyName">Name of the property</param>
		/// <param name="value">New value of the property</param>
		/// <param name="materialIndex">Material index on the renderer to set the property of</param>
		public static void SetProperty(this Renderer renderer, string propertyName, float value, int materialIndex = 0)
		{
			renderer.SetProperty(Shader.PropertyToID(propertyName), value, materialIndex);
		}

		/// <summary>
		/// Sets a property of a material using a PropertyBlock (i.e. without create new material instances)
		/// </summary>
		/// <param name="renderer">Renderer to set the property of</param>
		/// <param name="propertyHash">Hash of the property</param>
		/// <param name="value">New value of the property</param>
		/// <param name="materialIndex">Material index on the renderer to set the property of</param>
		public static void SetProperty(this Renderer renderer, int propertyHash, Color value, int materialIndex = 0)
		{
			if (materialIndex >= renderer.materials.Length)
			{
				Debug.LogWarning(
					$"Renderer {renderer.name} has {renderer.materials.Length.ToString()} materials. You are trying to access {materialIndex}. material.");
				return;
			}

			renderer.GetPropertyBlock(MaterialPropertyBlock, materialIndex);

			MaterialPropertyBlock.SetColor(propertyHash, value);

			renderer.SetPropertyBlock(MaterialPropertyBlock);
		}

		/// <summary>
		/// Sets a property of a material using a PropertyBlock (i.e. without create new material instances)
		/// </summary>
		/// <param name="renderer">Renderer to set the property of</param>
		/// <param name="propertyName">Name of the property</param>
		/// <param name="value">New value of the property</param>
		/// <param name="materialIndex">Material index on the renderer to set the property of</param>
		public static void SetProperty(this Renderer renderer, string propertyName, Color value, int materialIndex = 0)
		{
			renderer.SetProperty(Shader.PropertyToID(propertyName), value, materialIndex);
		}

		#endregion
	}
}