using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LoseWindow : MonoBehaviour
    {
        private Input _input;

        private void Awake()
        {
            _input = new Input();

            _input.Battle.Continue.performed += ContinuePerformed;

            _input.Enable();
        }

        private void ContinuePerformed(InputAction.CallbackContext obj)
        {
            StartCoroutine(ReplaySceneCoroutine());
        }

        private IEnumerator ReplaySceneCoroutine()
        {
            var levelLoader = FindObjectOfType<LevelLoader>(includeInactive: true);

            if (levelLoader == null)
            {
                yield break;
            }

            levelLoader.ReloadLevel();
        }
    }
}