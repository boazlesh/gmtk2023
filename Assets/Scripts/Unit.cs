using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private int _maxHealth;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Role _bodyRole;
        [SerializeField] private Role _hatRole;

        private int _health;

        private void Start()
        {
            _healthBar.SetMaxHealth(_maxHealth);

            SetHealth(_maxHealth);
        }

        public IEnumerator PlayRoutine()
        {
            _spriteRenderer.color = Color.magenta;

            yield return new WaitForSeconds(1);

            Debug.Log($"{name} - Play");

            _spriteRenderer.color = Color.white;

            yield return null;
        }

        public bool IsAlive() => _health > 0;

        public IEnumerator DamageRoutine(int damage)
        {
            SetHealth(Mathf.Max(_health - damage, 0));

            yield return null;
        }

        private void SetHealth(int health)
        {
            _health = health;
            _healthBar.SetHealth(_health);
        }

        [ContextMenu("Damage Ten")]
        private void DamageTen() => StartCoroutine(DamageRoutine(10));
    }
}