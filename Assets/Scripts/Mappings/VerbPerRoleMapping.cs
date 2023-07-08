using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using System;
using UnityEngine;

namespace Assets.Scripts.Mappings
{
    [Serializable]
    public class RoleToAbility
    {
        [SerializeField] private Role _role;
        [SerializeField] private Ability _ability;
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/VerbPerRoleMapping")]
    public class VerbPerRoleMapping : ScriptableObject
    {
        [NonReorderable] [SerializeField] private GenericDictionary<Verb, RoleToAbility[]> _mappings;
    }
}