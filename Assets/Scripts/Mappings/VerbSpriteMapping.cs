using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Mappings
{
    [CreateAssetMenu(menuName = "ScriptableObjects/VerbSpriteMapping")]
    public class VerbSpriteMapping : ScriptableObject
    {
        [NonReorderable] [SerializeField] public GenericDictionary<Verb, Sprite> _sprites;
    }
}