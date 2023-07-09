using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Ability _ability;
        private DateTime _startTime;
        private bool _didReachDestination;
        private Vector3 _sourcePosition;
        private Vector3 _targetPosition;

        public IEnumerator ProjectAbilityRoutine(Unit source, Unit target, Ability ability)
        {
            _ability = ability;
            _spriteRenderer.sprite = ability.ProjectileSprite;
            _startTime = DateTime.UtcNow;

            _sourcePosition = source.transform.position;
            _targetPosition = target.transform.position;

            yield return new WaitUntil(() => _didReachDestination);

            Destroy(gameObject);
        }

        public void Update()
        {
            float lerpAlpha = (float)(DateTime.UtcNow - _startTime).TotalSeconds / _ability.ProjectileTime;

            if (lerpAlpha >= 1)
            {
                _didReachDestination = true;
            }

            transform.position = Vector3.Lerp(_sourcePosition, _targetPosition, lerpAlpha);
        }
    }
}