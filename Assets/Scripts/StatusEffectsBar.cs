using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class StatusEffectsBar : MonoBehaviour
    {
        [SerializeField] private Image _border;
        [SerializeField] private Transform _contentHolder;
        [SerializeField] private StatusEffectDisplay _statusDisplayPrefab;

        public void SetStatusEffects(Ability[] abilities)
        {
            foreach (Transform child in _contentHolder)
            {
                Destroy(child.gameObject);
            }

            if (abilities.Length == 0)
            {
                _border.enabled = false;

                return;
            }

            _border.enabled = true;

            foreach (Ability ability in abilities)
            {
                AddStatusEffect(ability);
            }
        }

        private void AddStatusEffect(Ability ability)
        {
            StatusEffectDisplay statusEffectDisplay = Instantiate(_statusDisplayPrefab, _contentHolder);
            statusEffectDisplay.SetStatusEffect(ability);
        }
    }
}