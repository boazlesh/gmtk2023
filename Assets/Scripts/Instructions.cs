using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class Instructions : MonoBehaviour
    {
        [SerializeField] private LevelLoader _levelLoader;
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] List<Transform> _parts;
        private int _currentPart = 0;

        private Input _input;

        private void Awake()
        {
            _input = new Input();

            _input.Battle.Continue.performed += ContinuePerformed;

            _input.Enable();
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void ContinuePerformed(InputAction.CallbackContext obj)
        {
            if (_currentPart == _parts.Count - 1)
            {
                StartCoroutine(_audioSource.FadeOut(1f));
                _levelLoader.LoadNextLevel();

                return;
            }

            _parts[_currentPart].gameObject.SetActive(false);
            _currentPart++;
            _parts[_currentPart].gameObject.SetActive(true);
        }
    }
}
