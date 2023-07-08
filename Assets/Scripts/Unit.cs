using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
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

        public IEnumerator ConjureIntentionRoutine()
        {
            _bodySpriteRenderer.color = Color.green;

            yield return new WaitForSeconds(0.25f);

            Debug.Log($"{name} - Conjure");

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

            Debug.Log($"{name} - Damaged");

            _bodySpriteRenderer.color = Color.white;

            yield return new WaitForSeconds(0.5f);

            SetHealth(Mathf.Max(_health - damage, 0));

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

        public IEnumerable<Unit> GetNeighbors()
        {
            throw new System.NotImplementedException();
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