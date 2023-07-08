using UnityEngine;

namespace Assets.Scripts
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private HealthBar _healthBar;

        private int _health;

        private void Start()
        {
            _healthBar.SetMaxHealth(_maxHealth);

            SetHealth(_maxHealth);
        }

        public void Damage(int damage)
        {
            SetHealth(Mathf.Max(_health - damage, 0));
        }

        private void SetHealth(int health)
        {
            _health = health;
            _healthBar.SetHealth(_health);
        }

        [ContextMenu("Damage Ten")]
        private void DamageTen() => Damage(10);
    }
}