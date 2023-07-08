using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class WinWindow : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Transform _rewardWindow;

        private Input _input;

        private void Awake()
        {
            _input = new Input();

            _input.Battle.Continue.performed += ContinuePerformed;

            _input.Enable();
        }

        private void ContinuePerformed(InputAction.CallbackContext obj)
        {
            StartCoroutine(NextSceneCoroutine());
        }

        private IEnumerator NextSceneCoroutine()
        {
            var levelLoader = FindObjectOfType<LevelLoader>(includeInactive: true);

            if (levelLoader == null)
            {
                yield break;
            }

            levelLoader.LoadNextLevel();
        }
    }
}