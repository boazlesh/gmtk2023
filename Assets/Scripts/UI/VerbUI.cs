using Assets.Scripts.Enums;
using Assets.Scripts.Mappings;
using UnityEngine;

namespace Assets.Scripts
{
    public class VerbUI : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private VerbSpriteMapping _verbSpriteMapping;

		private Verb _verb;

		public Verb Verb => _verb;

		public void SetVerb(Verb verb)
		{
			_verb = verb;
			_spriteRenderer.sprite = _verbSpriteMapping._sprites[verb];
		}
	}
}