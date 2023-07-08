using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using System;
using UnityEngine;

namespace Assets.Scripts.Mappings
{
    [Serializable]
    public class VerbToAbility
    {
        [NonReorderable] [SerializeField] private GenericDictionary<Verb, Ability> _verbToAbility;

        public GenericDictionary<Verb, Ability> Mapping { get { return _verbToAbility; } }
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/VerbPerRoleMapping")]
    public class VerbPerRoleMapping : ScriptableObject
    {
        [NonReorderable] [SerializeField] private GenericDictionary<Role, VerbToAbility> _roles;

        public GenericDictionary<Role, VerbToAbility> Mapping { get { return _roles; } }
    }
}