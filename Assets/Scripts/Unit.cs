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
        [SerializeField] private Transform _hatFloatPosition;

        private int _health;
        private Intention _currentIntention;
        private Vector3 _originalHatPosition;

        public Sprite BodySprite
        {
            get => _bodySpriteRenderer.sprite;
            set => _bodySpriteRenderer.sprite = value;
        }

        public Sprite HatSprite
        {
            get => _hatSpriteRenderer.sprite;
            set => _hatSpriteRenderer.sprite = value;
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

            yield return new WaitForSeconds(0.4f);

            Debug.Log($"{name} - Conjure");

            _bodySpriteRenderer.color = Color.white;

            _currentIntention = PlanIntention();

            yield return null;
        }

        public IEnumerator PlayInentionRoutine()
        {
            _bodySpriteRenderer.color = Color.magenta;

            yield return new WaitForSeconds(0.4f);

            Debug.Log($"{name} - Play");

            _bodySpriteRenderer.color = Color.white;

            _currentIntention = null;

            yield return null;
        }

        public bool IsAlive() => _health > 0;

        public IEnumerator DamageRoutine(int damage)
        {
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

        private void SetHealth(int health)
        {
            _health = Mathf.Clamp(health, 0, _maxHealth);
            _healthBar.SetHealth(_health);
        }

        [ContextMenu("Damage Ten")]
        private void DamageTen() => StartCoroutine(DamageRoutine(10));

        private Intention PlanIntention()
        {
            Verb verb = (Verb)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Verb)).Length);

            //int teamSize = (verb)

            return new Intention(Verb.Offensive, 0);
        }
    }
}