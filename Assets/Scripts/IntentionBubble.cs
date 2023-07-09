using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts
{
    public class IntentionBubble : MonoBehaviour
    {
        [SerializeField] private VerbUI _verbUI;
        [SerializeField] private AbilityUI _abilityUI;
        [SerializeField] private TargetUnitUI _targetUnitUI;
        [SerializeField] private ModifiedHealthNumberIndicator _healthNumberIndicator;

        public void SetIntention(Verb verb, Ability ability, Unit unit)
        {
            _verbUI.SetVerb(verb);
            _abilityUI.SetAbility(ability);
            _targetUnitUI.SetTarget(unit);

            _healthNumberIndicator.gameObject.SetActive(true);
            if (ability.Damage != 0)
            {
                _healthNumberIndicator.IndicateModifiedHealthNumber(ability.Damage);
            }
            else if (ability.AddsDamageModifier)
            {
                _healthNumberIndicator.IndicateModifiedHealthNumber(ability.DamageModifier);
            }
            else
            {
                _healthNumberIndicator.gameObject.SetActive(false);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}