using Assets.Scripts.Enums;
using System.Collections;
using System.Dynamic;
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

        public Unit ResolveTargetUnit()
        {
            return GameManager.Instance.GetUnitByIntention(this);
        }

        public IEnumerator PerformIntetionRoutine()
        {
            yield return new WaitForSeconds(0.5f);

            Ability ability = ResolveAbility();
            Unit targetUnit = ResolveTargetUnit();

            if (!ability.HitEveryone)
            {
                yield return PerformAbilityReoutine(ability, targetUnit);
            }
            else
            {
                foreach (Unit unit in targetUnit.GetTeam())
                {
                    yield return PerformAbilityReoutine(ability, unit);
                }
            }
        }

        private IEnumerator PerformAbilityReoutine(Ability ability, Unit targetUnit)
        {
            yield return targetUnit.DamageRoutine(ability.Damage);

            if (ability.NeighborDamage != 0)
            {
                foreach (Unit neighbor in targetUnit.GetNeighbors())
                {
                    yield return neighbor.DamageRoutine(ability.NeighborDamage);
                }
            }
        }
    }
}