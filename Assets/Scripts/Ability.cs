using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Ability")]
    public class Ability : ScriptableObject
    {
        public Sprite IconSprite;

        public int Damage;
        public int NeighborDamage;
        public bool HitEveryone;

        public bool AddsDamageModifier;
        public float DamageModifier;
        public bool AddsNeighborDamageModifier;
        public float NeighborDamageModifier;
        public bool IsDamageModifierPersistent;
    }
}