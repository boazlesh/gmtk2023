using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class RoutineExtensions
	{
		public static IEnumerator WhenAllRoutine(this MonoBehaviour monoBehaviour, params IEnumerator[] routines)
		{
			List<Coroutine> coroutines = new List<Coroutine>(routines.Length);

			foreach (IEnumerator routine in routines)
			{
				coroutines.Add(monoBehaviour.StartCoroutine(routine));
			}

			foreach (Coroutine coroutine in coroutines)
			{
				yield return coroutine;
			}
		}
	}
}