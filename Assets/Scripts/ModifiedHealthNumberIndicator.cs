using Assets.Scripts.Utils;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class ModifiedHealthNumberIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Color _neutralColor;
        [SerializeField] private Color _damagedColor;
        [SerializeField] private Color _healedColor;
        [SerializeField] private bool _notAnimated;

        private Animator _animator;

        private void Awake()
        {
            _animator = _text.GetComponent<Animator>();

            if (_notAnimated)
            {
                _animator.enabled = false;
            }
        }

        public void IndicateModifiedHealthNumber(int modifiedHealthNumber)
        {
            if (modifiedHealthNumber == 0)
            {
                _text.color = _neutralColor;
            }
            else
            {
                _text.color = modifiedHealthNumber > 0 ? _damagedColor : _healedColor;
            }

            _text.text = Math.Abs(modifiedHealthNumber).ToString();

            if (!_notAnimated)
            {
                StartCoroutine(DestoryAfterAnimation());
            }
        }

        public void IndicateModifiedHealthNumber(float modifiedHealthNumber)
        {
            if (modifiedHealthNumber == 1)
            {
                _text.color = _neutralColor;
            }
            else
            {
                _text.color = modifiedHealthNumber > 1 ? _damagedColor : _healedColor;
            }

            _text.text = $"*{ Mathf.Abs(modifiedHealthNumber)}";

            if (!_notAnimated)
            {
                StartCoroutine(DestoryAfterAnimation());
            }
        }

        private IEnumerator DestoryAfterAnimation()
        {
            yield return _animator.WaitForAnimationToEndRoutine();

            Destroy(gameObject);
        }
    }
}
