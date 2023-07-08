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
        [SerializeField] private ActionSpriteMapping _actionSpriteMapping;

        public void SetIntention(Verb verb, ActionType actionType)
        {
            _verbSpriteRenderer.sprite = _verbSpriteMapping._sprites[verb];
            _actionSpriteRenderer.sprite = _actionSpriteMapping._sprites[actionType];
            // TODO: Target icon
        }
    }
}