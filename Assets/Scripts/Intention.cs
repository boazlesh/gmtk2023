using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Intention
    {
        public Unit Originator { get; set; }
        public Verb Verb { get; set; }
        public int TargetIndex { get; set; }

        private Ability _bossAbility;

        public Intention(Unit originator, Verb verb, int targetIndex)
        {
            Originator = originator;
            Verb = verb;
            TargetIndex = targetIndex;
        }

        public Ability ResolveAbility()
        {
            if (Originator.BodyRole == Role.Boss)
            {
                if (_bossAbility != null)
                {
                    return _bossAbility;
                }

                _bossAbility = RollBossAbility();

                return _bossAbility;
            }

            return GameManager.Instance._verbPerRoleMapping.Mapping[Originator.HatRole].Mapping[Verb];
        }

        public Unit ResolveTargetUnit()
        {
            return GameManager.Instance.GetUnitByIntention(this);
        }

        public IEnumerator PerformIntetionRoutine()
        {
            Ability ability = ResolveAbility();

            yield return Originator.PlayAbilityAnimationRoutine(ability);

            Unit targetUnit = ResolveTargetUnit();

            if (!ability.HitEveryone)
            {
                yield return PerformAbilityReoutine(ability, targetUnit);
            }
            else
            {
                Unit[] team = targetUnit.GetTeam();
                for (int i = 0; i < team.Length; i++)
                {
                    Unit unit = team[i];
                    IEnumerator enumerator = PerformAbilityReoutine(ability, unit);
                    if (!ability.HitEveryoneDontWaitForEachHitToCompleteBeforeNext || i == team.Length - 1)
                    {
                        yield return enumerator;
                    }
                    else
                    {
                        Originator.StartCoroutine(enumerator);
                        yield return new WaitForSeconds(ability.HitEveryoneCustomWaitBetweenEachHit);
                    }
                }
            }
        }

        private IEnumerator PerformAbilityReoutine(Ability ability, Unit targetUnit)
        {
            if (ability.StartClip != null)
            {
                Originator.AudioSource.PlayOneShot(ability.StartClip);
            }
            if (ability.ProjectileSprite != null)
            {
                Projectile projectile = Originator.CreateProjectile();
                yield return projectile.ProjectAbilityRoutine(Originator, targetUnit, ability);
            }

            if (ability.IsSwap)
            {
                yield return GameManager.Instance.SwapUnitsRoutine(Originator, targetUnit);

                yield break;
            }

            if (ability.Damage != 0)
            {
                yield return targetUnit.DamageRoutine(ability.Damage);
            }

            if (ability.NeighborDamage != 0)
            {
                foreach (Unit neighbor in targetUnit.GetNeighbors())
                {
                    yield return neighbor.DamageRoutine(ability.NeighborDamage);
                }
            }

            if (ability.AddsDamageModifier)
            {
                yield return targetUnit.AddDamageModifier(ability);
            }

            if (ability.AddsNeighborDamageModifier)
            {
                foreach (Unit neighbor in targetUnit.GetNeighbors())
                {
                    yield return neighbor.AddDamageModifier(ability);
                }
            }
        }

        private Ability RollBossAbility()
        {
            List<Ability> abilities;

            switch (Verb)
            {
                case Verb.Offensive:
                    abilities = GameManager.Instance._bossOffensiveAbilities;
                    break;
                case Verb.Defensive:
                    abilities = GameManager.Instance._bossDefensiveAbilities;
                    break;
                case Verb.Special:
                    abilities = GameManager.Instance._bossSpecialAbilities;
                    break;
                default:
                    throw new System.NotSupportedException($"{Verb} not supported");
            }

            int targetIndex = Random.Range(0, abilities.Count);

            return abilities[targetIndex];
        }
    }
}