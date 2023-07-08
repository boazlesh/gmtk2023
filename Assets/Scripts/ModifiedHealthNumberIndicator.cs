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

        private Animator _animator;

        private void Awake()
        {
            _animator = _text.GetComponent<Animator>();
        }

        public void IndicateModifiedHealthNumber(int modifiedHealthNumber)
        {
            if (modifiedHealthNumber == 0)
            {
                _text.faceColor = _neutralColor;
            }
            else
            {
                _text.faceColor = modifiedHealthNumber > 0 ? _damagedColor : _healedColor;
            }

            _text.text = Math.Abs(modifiedHealthNumber).ToString();

            StartCoroutine(DestoryAfterAnimation());
        }

        private IEnumerator DestoryAfterAnimation()
        {
            yield return _animator.WaitForAnimationToEndRoutine();

            Destroy(gameObject);
        }
    }
}
