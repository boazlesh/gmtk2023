using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils
{
	public static class AudioSourceExtensions
	{
		public static IEnumerator FadeOut(this AudioSource audioSource, float fadeTimeSeconds)
		{
			float startVolume = audioSource.volume;

			while (audioSource.volume > 0)
			{
				audioSource.volume -= startVolume * Time.deltaTime / fadeTimeSeconds;

				yield return null;
			}

			audioSource.Stop();
			audioSource.volume = startVolume;
		}
	}
}
