using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Mappings
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ActionSpriteMapping")]
    public class ActionSpriteMapping : ScriptableObject
    {
        [NonReorderable] [SerializeField] public GenericDictionary<ActionType, Sprite> _sprites;
    }
}