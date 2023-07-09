using Assets.Scripts.Enums;
using Assets.Scripts.Mappings;
using UnityEngine;

namespace Assets.Scripts
{
    public class IntentionBubble : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _verbSpriteRenderer;
        [SerializeField] private SpriteRenderer _actionSpriteRenderer;
        [SerializeField] private SpriteRenderer _targetSpriteRenderer;
        [SerializeField] private VerbSpriteMapping _verbSpriteMapping;
        [SerializeField] private ModifiedHealthNumberIndicator _healthNumberIndicator;

        public void SetIntention(Verb verb, Ability ability, Unit unit)
        {
            _verbSpriteRenderer.sprite = _verbSpriteMapping._sprites[verb];
            _actionSpriteRenderer.sprite = ability.IconSprite;
            _targetSpriteRenderer.sprite = unit.IconSprite;
            _targetSpriteRenderer.flipX = !unit.IsPlayerUnit;

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