using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class StatusEffectDisplay : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private ModifiedHealthNumberIndicator _modifiedHealthNumberIndicator;

        public void SetStatusEffect(Ability ability)
        {
            _image.sprite = ability.IconSprite;
            _modifiedHealthNumberIndicator.IndicateModifiedHealthNumber(ability.DamageModifier);
        }
    }
}
