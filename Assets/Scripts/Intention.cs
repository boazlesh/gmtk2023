using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Intention
    {
        public Unit Originator { get; set; }
        public Verb Verb { get; set; }
        public int TargetIndex { get; set; }

        public Intention(Unit originator, Verb verb, int targetIndex)
        {
            Originator = originator;
            Verb = verb;
            TargetIndex = targetIndex;
        }

        public Ability ResolveAbility()
        {
            return GameManager.Instance._verbPerRoleMapping.Mapping[Originator.HatRole].Mapping[Verb];
        }

        public IEnumerator PerformIntetion()
        {
            yield return new WaitForSeconds(0.5f);

            Ability ability = ResolveAbility();

            yield return GameManager.Instance.GetUnitByIntention(this).DamageRoutine(ability.Damage);
        }
    }
}