using Assets.Scripts.Enums;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _bodySpriteRenderer;
        [SerializeField] private SpriteRenderer _hatSpriteRenderer;
        [SerializeField] private int _maxHealth;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Role _bodyRole;
        [SerializeField] private Role _hatRole;

        private int _health;
        private Intention _currentIntention;
        public bool _isPlayerUnit;

        private void Start()
        {
            _healthBar.SetMaxHealth(_maxHealth);

            SetHealth(_maxHealth);
        }

        public IEnumerator ConjureIntentionRoutine()
        {
            _bodySpriteRenderer.color = Color.green;

            yield return new WaitForSeconds(0.25f);

            Debug.Log($"{name} - Conjure");

            _bodySpriteRenderer.color = Color.white;

            _currentIntention = PlanIntention();

            yield return null;
        }

        public IEnumerator PlayInentionRoutine()
        {
            _bodySpriteRenderer.color = Color.magenta;

            yield return new WaitForSeconds(0.5f);

            yield return GameManager.Instance.GetUnitByIntention(_currentIntention).DamageRoutine(10);

            Debug.Log($"{name} - Play");

            _bodySpriteRenderer.color = Color.white;

            _currentIntention = null;

            yield return null;
        }

        public bool IsAlive() => _health > 0;

        public IEnumerator DamageRoutine(int damage)
        {
            _bodySpriteRenderer.color = Color.red;

            yield return new WaitForSeconds(0.5f);

            Debug.Log($"{name} - Damaged");

            _bodySpriteRenderer.color = Color.white;

            SetHealth(Mathf.Max(_health - damage, 0));

            yield return null;
        }

        private void SetHealth(int health)
        {
            _health = Mathf.Clamp(health, 0, _maxHealth);
            _healthBar.SetHealth(_health);
        }

        [ContextMenu("Damage Ten")]
        private void DamageTen() => StartCoroutine(DamageRoutine(10));

        private Intention PlanIntention()
        {
            Verb verb = (Verb)UnityEngine.Random.Range(0, 2); // no special -- TODO: special bar, and require body-hat match, which overrides the regular intention?
            Unit[] targetUnits = GetVerbPossibleTargetTeam(verb);
            int targetIndex = UnityEngine.Random.Range(0, targetUnits.Length);

            int randomDirection = UnityEngine.Random.Range(0, 2);
            randomDirection = (randomDirection == 0) ? -1 : 1;
            while (!targetUnits[targetIndex].IsAlive())
            {
                targetIndex += randomDirection;
            }

            return new Intention(this, Verb.Offensive, targetIndex);
        }

        public Unit[] GetVerbPossibleTargetTeam(Verb verb)
        {
            Unit[] friendlyUnits = _isPlayerUnit ? GameManager.Instance._playerUnits : GameManager.Instance._enemyUnits;
            Unit[] enemyUnits = _isPlayerUnit ? GameManager.Instance._enemyUnits : GameManager.Instance._playerUnits;
            Unit[] targetUnits = (verb == Verb.Defensive) ? friendlyUnits : enemyUnits;

            return targetUnits;
        }
    }
}