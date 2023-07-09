using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private Transform _hatFloatPosition;
        [SerializeField] private IntentionBubble _intentionBubble;
        [SerializeField] private Sprite _iconSprite;
        [SerializeField] private Animator _animator;
        [SerializeField] private StatusEffectsBar _statusEffectsBar;

        private int _health;
        private Intention _currentIntention;
        private Vector3 _originalHatPosition;
        private Quaternion _originalHatRotation;
        private List<Ability> _persistantDamageModifiers = new List<Ability>();
        private List<Ability> _turnDamageModifiers = new List<Ability>();
        private bool _idleStarted;

        public Sprite IconSprite => _iconSprite;

        public int UnitIndex { get; set; }

        public bool IsPlayerUnit { get; set; }

        public Vector3 OriginalHatPosition => _originalHatPosition;

        public Quaternion OriginalHatRotation => _originalHatRotation;

        public Sprite BodySprite
        {
            get => _bodySpriteRenderer.sprite;
            set => _bodySpriteRenderer.sprite = value;
        }

        public Transform HatContainer => _hatSpriteRenderer.transform.parent;

        public SpriteRenderer HatSpriteRenderer => _hatSpriteRenderer;

        public Role BodyRole
        {
            get => _bodyRole;
            set => _bodyRole = value;
        }

        public Role HatRole
        {
            get => _hatRole;
            set => _hatRole = value;
        }

        private void Awake()
        {
            _healthBar.SetMaxHealth(_maxHealth);

            SetHealth(_maxHealth);
        }

        public IEnumerator PlanIntentionRoutine()
        {
            if (!IsAlive())
            {
                yield break;
            }

            Debug.Log($"{name} - Plan");

            _currentIntention = PlanIntention();
            InvalidateIntention();
            _intentionBubble.Show();

            if (!_idleStarted)
            {
                _animator.SetTrigger("startIdle");
                _idleStarted = true;
            }

            yield return new WaitForSeconds(0.6f);
        }

        public IEnumerator PlayInentionRoutine()
        {
            yield return _currentIntention.PerformIntetionRoutine();

            Debug.Log($"{name} - Play");

            _currentIntention = null;

            _intentionBubble.Hide();

            yield return null;
        }

        public IEnumerator PlayAbilityAnimationRoutine(Ability ability)
        {
            _animator.SetTrigger("attack");
            yield return new WaitForSeconds(0.5f);
        }

        public bool IsAlive() => _health > 0;

        public IEnumerator DamageRoutine(int damage)
        {
            if (!IsAlive())
            {
                yield break;
            }

            _bodySpriteRenderer.color = (damage > 0) ? Color.red : Color.green;

            yield return new WaitForSeconds(0.5f);

            Debug.Log($"{name} - Damaged");

            _bodySpriteRenderer.color = Color.white;

            float actualDamage = damage;
            foreach (float modifier in _persistantDamageModifiers.Concat(_turnDamageModifiers).Select(ability => ability.DamageModifier))
            {
                actualDamage *= modifier;
            }
            int actualDamageInt = (int)actualDamage;

            SetHealth(Mathf.Max(_health - actualDamageInt, 0));

            if (_health == 0)
            {
                Die();
            }

            Instantiate(GameManager.Instance._modifiedHealthNumberIndicatorPrefab, transform.position, Quaternion.identity).IndicateModifiedHealthNumber(actualDamageInt);

            yield return null;
        }

        public IEnumerator FloatHatRoutine()
        {
            Debug.Log($"{name} - Float hat");

            _originalHatPosition = HatContainer.position;
            _originalHatRotation = HatContainer.rotation;
            HatContainer.position = _hatFloatPosition.position;

            yield return null;
        }

        public IEnumerator UnfloatHatRoutine()
        {
            Debug.Log($"{name} - Unfloat hat");

            HatContainer.position = _originalHatPosition;
            HatContainer.rotation = _originalHatRotation;

            yield return null;
        }

        public Unit[] GetVerbPossibleTargetTeam(Verb verb)
        {
            Unit[] friendlyUnits = IsPlayerUnit ? GameManager.Instance._playerUnits : GameManager.Instance._enemyUnits;
            Unit[] enemyUnits = IsPlayerUnit ? GameManager.Instance._enemyUnits : GameManager.Instance._playerUnits;
            Unit[] targetUnits = (verb == Verb.Defensive) ? friendlyUnits : enemyUnits;

            return targetUnits;
        }

        public Unit[] GetTeam()
        {
            return GetVerbPossibleTargetTeam(Verb.Defensive); // hack to get teammates
        }

        public IEnumerable<Unit> GetNeighbors()
        {
            Unit[] teammates = GetTeam();

            if (UnitIndex - 1 >= 0)
            {
                yield return teammates[UnitIndex - 1];
            }
            if (UnitIndex + 1 < teammates.Length)
            {
                yield return teammates[UnitIndex + 1];
            }
        }

        public IEnumerator AddDamageModifier(Ability ability)
        {
            _bodySpriteRenderer.color = (ability.DamageModifier > 1) ? new Color(0.75f, 0.5f, 0.25f, 1f) : Color.gray;

            Debug.Log($"{name} - Add Damage Modifier");

            yield return new WaitForSeconds(0.25f);

            _bodySpriteRenderer.color = Color.white;

            if (ability.IsDamageModifierPersistent)
            {
                _persistantDamageModifiers.Add(ability);
            }
            else
            {
                _turnDamageModifiers.Add(ability);
            }

            _statusEffectsBar.SetStatusEffects(_persistantDamageModifiers.Concat(_turnDamageModifiers).ToArray());

            yield return null;
        }

        public void EndTurn()
        {
            ClearTunrModifiers();
        }

        public void InvalidateIntention()
        {
            if (_currentIntention == null)
            {
                return;
            }

            _intentionBubble.SetIntention(_currentIntention.Verb, _currentIntention.ResolveAbility(), _currentIntention.ResolveTargetUnit());
        }

        private void ClearTunrModifiers()
        {
            _turnDamageModifiers.Clear();
            _statusEffectsBar.SetStatusEffects(_persistantDamageModifiers.Concat(_turnDamageModifiers).ToArray());
        }

        private void SetHealth(int health)
        {
            _health = Mathf.Clamp(health, 0, _maxHealth);
            _healthBar.SetHealth(_health);
        }

        private void Die()
        {
            _turnDamageModifiers.Clear();
            _persistantDamageModifiers.Clear();
            _statusEffectsBar.SetStatusEffects(_persistantDamageModifiers.Concat(_turnDamageModifiers).ToArray());
            _intentionBubble.Hide();

            _animator.SetTrigger("die");

            StopAllCoroutines();
            GameManager.Instance.CheckWinLose();
        }

        [ContextMenu("Damage Ten")]
        private void DamageTen() => StartCoroutine(DamageRoutine(10));

        private Intention PlanIntention()
        {
            Verb verb = (Verb)Random.Range(0, 2); // no special -- TODO: special bar, and require body-hat match, which overrides the regular intention?
            Unit[] targetUnits = GetVerbPossibleTargetTeam(verb);
            int targetIndex = Random.Range(0, targetUnits.Length);

            int randomDirection = Random.Range(0, 2);
            randomDirection = (randomDirection == 0) ? -1 : 1;
            while (!targetUnits[targetIndex].IsAlive())
            {
                targetIndex = (targetIndex + randomDirection) % targetUnits.Length;
                if (targetIndex == -1)
                {
                    targetIndex = targetUnits.Length;
                }
            }

            return new Intention(this, verb, targetIndex);
        }
    }
}