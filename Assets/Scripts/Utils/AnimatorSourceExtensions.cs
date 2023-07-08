using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class AnimatorExtensions
    {
        public static IEnumerator PlayOneShotRoutine(this Animator animator, string stateName)
        {
            if (!animator.isActiveAndEnabled)
            {
                yield break;
            }

            animator.Play(stateName);

            yield return WaitForAnimationToEndRoutine(animator);
        }

        public static IEnumerator WaitForAnimationToEndRoutine(this Animator animator)
        {
            // from: https://answers.unity.com/questions/1208395/animator-wait-until-animation-finishes.html
            AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(animationInfo.length + animationInfo.normalizedTime);
        }
    }
}
