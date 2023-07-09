﻿using System;
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
        private bool _shouldFlip;
        private float _originalAlpha;

        public IEnumerator ProjectAbilityRoutine(Unit source, Unit target, Ability ability)
        {
            _ability = ability;
            _spriteRenderer.sprite = ability.ProjectileSprite;
            _startTime = DateTime.UtcNow;

            _sourcePosition = (!ability.UseCustomSource ? source.transform.position : (ability.CustomSourcePointRelativeToTarget.sqrMagnitude == 0 ? (source.transform.position + ability.CustomSourcePointRelativeToSource) : (target.transform.position + ability.CustomSourcePointRelativeToTarget)));
            _targetPosition = target.transform.position;
            _shouldFlip = !source.IsPlayerUnit && !ability.DontFlipForEnemy;
            _originalAlpha = _spriteRenderer.color.a;

            yield return new WaitUntil(() => _didReachDestination);

            Destroy(gameObject);
        }

        public void Update()
        {
            float lerpAlpha = (float)(DateTime.UtcNow - _startTime).TotalSeconds / _ability.ProjectileTime;

            if (lerpAlpha >= 0.93f)
            {
                _didReachDestination = true;

                return;
            }

            transform.position = Vector3.Lerp(_sourcePosition, _targetPosition, _ability.ProjectileCurve.Evaluate(lerpAlpha));
            transform.rotation = Quaternion.Euler(Vector3.Lerp(_ability.SourceRotation, _ability.TargetRotation, _ability.ProjectileRotationCurve.Evaluate(lerpAlpha)) + new Vector3(0, 0, _shouldFlip ? 180 : 0));
            transform.localScale = Vector3.Lerp(_ability.SourceScale, _ability.TargetScale, _ability.ProjectileScaleCurve.Evaluate(lerpAlpha));

            if (_ability.CustomFade)
            {
                _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _originalAlpha * _ability.ProjectileFadeCurve.Evaluate(lerpAlpha));
            }
        }
    }
}