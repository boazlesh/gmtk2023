using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Ability")]
    public class Ability : ScriptableObject
    {
        public Sprite IconSprite;
        public string DisplayName;
        public string Description;

        public int Damage;
        public int NeighborDamage;
        public bool HitEveryone;

        public bool AddsDamageModifier;
        public float DamageModifier;
        public bool AddsNeighborDamageModifier;
        public bool IsDamageModifierPersistent;

        public bool HitEveryoneDontWaitForEachHitToCompleteBeforeNext;
        public float HitEveryoneCustomWaitBetweenEachHit;

        public bool IsSwap;

        public Sprite ProjectileSprite;
        public float ProjectileTime;
        public AnimationCurve ProjectileCurve;
        public Vector3 SourceRotation;
        public Vector3 TargetRotation;
        public AnimationCurve ProjectileRotationCurve;
        public bool DontFlipForEnemy;
        public bool UseCustomSource;
        public Vector3 CustomSourcePointRelativeToTarget;
        public Vector3 CustomSourcePointRelativeToSource;
        public Vector3 SourceScale;
        public Vector3 TargetScale;
        public AnimationCurve ProjectileScaleCurve;
        public bool CustomFade;
        public AnimationCurve ProjectileFadeCurve;

        public AudioClip StartClip;
        public AudioClip HitClip;
    }
}