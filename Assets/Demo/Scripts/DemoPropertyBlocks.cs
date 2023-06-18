using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ExtraTools.Test.Scripts
{
	public class DemoPropertyBlocks : MonoBehaviour
	{
		[SerializeField] private Renderer[] renderers;

		private static readonly int colorHash = Shader.PropertyToID("_Color");
		private const string colorName = "_Color";

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				ChangeColor();
			}

			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();

				for (var i = 0; i < 100000; i++)
				{
					ChangeColorsHash();
				}

				stopwatch.Stop();
				Debug.Log(
					$"It took {stopwatch.Elapsed.Milliseconds.ToString()}ms to change color of {renderers.Length.ToString()} 100000 times using hash.");
			}

			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();

				for (var i = 0; i < 100000; i++)
				{
					ChangeColorsString();
				}

				stopwatch.Stop();
				Debug.Log(
					$"It took {stopwatch.Elapsed.Milliseconds.ToString()}ms to change color of {renderers.Length.ToString()} 100000 times using string.");
			}
		}

		private void ChangeColor()
		{
			foreach (Renderer rend in renderers)
			{
				rend.SetProperty(colorHash, Random.ColorHSV());
			}
		}

		private void ChangeColorsHash()
		{
			foreach (Renderer rend in renderers)
			{
				rend.SetProperty(colorHash, Color.grey);
			}
		}

		private void ChangeColorsString()
		{
			foreach (Renderer rend in renderers)
			{
				rend.SetProperty(colorName, Color.grey);
			}
		}
	}
}