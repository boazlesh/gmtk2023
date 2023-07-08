using Assets.Scripts.Enums;
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

        private int _health;
        private Intention _currentIntention;
        private Vector3 _originalHatPosition;
        private List<float> _persistantDamageModifiers = new List<float>();
        private List<float> _turnDamageModifiers = new List<float>();

        public Sprite IconSprite => _iconSprite;

        public int UnitIndex { get; set; }

        public bool IsPlayerUnit { get; set; }

        public Vector3 OriginalHatPosition => _originalHatPosition;

        public Sprite BodySprite
        {
            get => _bodySpriteRenderer.sprite;
            set => _bodySpriteRenderer.sprite = value;
        }

        public SpriteRenderer HatSpriteRenderer
        {
            get => _hatSpriteRenderer;
        }

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

        private void Start()
        {
            _originalHatPosition = _hatSpriteRenderer.transform.position;

            _healthBar.SetMaxHealth(_maxHealth);

            SetHealth(_maxHealth);
        }

        public IEnumerator PlanIntentionRoutine()
        {
            _bodySpriteRenderer.color = Color.cyan;

            yield return new WaitForSeconds(0.25f);

            Debug.Log($"{name} - Plan");

            _bodySpriteRenderer.color = Color.white;

            _currentIntention = PlanIntention();

            _intentionBubble.SetIntention(_currentIntention.Verb, _currentIntention.ResolveAbility(), _currentIntention.ResolveTargetUnit());
            _intentionBubble.Show();

            yield return null;
        }

        public IEnumerator PlayInentionRoutine()
        {
            _bodySpriteRenderer.color = Color.magenta;

            yield return _currentIntention.PerformIntetionRoutine();

            Debug.Log($"{name} - Play");

            _bodySpriteRenderer.color = Color.white;

            _currentIntention = null;

            _intentionBubble.Hide();

            yield return null;
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
            foreach (float modifier in _persistantDamageModifiers.Concat(_turnDamageModifiers))
            {
                actualDamage *= modifier;
            }

            SetHealth(Mathf.Max(_health - (int)actualDamage, 0));

            yield return null;
        }

        public IEnumerator FloatHatRoutine()
        {
            Debug.Log($"{name} - Float hat");

            _hatSpriteRenderer.transform.position = _hatFloatPosition.position;

            yield return null;
        }

        public IEnumerator UnfloatHatRoutine()
        {
            Debug.Log($"{name} - Unfloat hat");

            _hatSpriteRenderer.transform.position = _originalHatPosition;

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

        public IEnumerator AddDamageModifier(float modifier, bool isPersistent)
        {
            _bodySpriteRenderer.color = (modifier > 1) ? new Color(0.75f, 0.5f, 0.25f, 1f) : Color.gray;

            Debug.Log($"{name} - Add Damage Modifier");

            yield return new WaitForSeconds(0.25f);

            _bodySpriteRenderer.color = Color.white;

            if (isPersistent)
            {
                _persistantDamageModifiers.Add(modifier);
            }
            else
            {
                _turnDamageModifiers.Add(modifier);
            }

            yield return null;
        }

        public void EndTurn()
        {
            ClearTunrModifiers();
        }

        private void ClearTunrModifiers()
        {
            _turnDamageModifiers.Clear();
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
            Verb verb = (Verb)Random.Range(0, 2); // no special -- TODO: special bar, and require body-hat match, which overrides the regular intention?
            Unit[] targetUnits = GetVerbPossibleTargetTeam(verb);
            int targetIndex = Random.Range(0, targetUnits.Length);

            int randomDirection = Random.Range(0, 2);
            randomDirection = (randomDirection == 0) ? -1 : 1;
            while (!targetUnits[targetIndex].IsAlive())
            {
                targetIndex += randomDirection;
            }

            return new Intention(this, verb, targetIndex);
        }
    }
}