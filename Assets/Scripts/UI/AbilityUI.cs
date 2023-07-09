using UnityEngine;

namespace Assets.Scripts
{
    public class AbilityUI : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;

		private Ability _ability;

		public Ability Ability => _ability;

		public void SetAbility(Ability ability)
		{
			_ability = ability;
			_spriteRenderer.sprite = ability.IconSprite;
		}
	}
}