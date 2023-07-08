using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Ability")]
    public class Ability : ScriptableObject
    {
        public int Damage;

        public Sprite IconSprite;
    }
}