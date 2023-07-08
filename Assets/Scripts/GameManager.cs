using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public Unit[] _playerUnits;
        [SerializeField] public Unit[] _enemyUnits;
        [SerializeField] private Button _fightButton;

        private Input _input;

        private void Awake()
        {
            _input = new Input();

            _input.Battle.MouseClick.performed += MouseClickPerformed;

            _input.Enable();
        }

        public void Start()
        {
            _fightButton.interactable = false;
            BuildIntentions();
        }

        public void BuildIntentions()
        {
            StartCoroutine(BuildIntentionsRoutine());
        }

        public void SwapHats()
        {
            StartCoroutine(SwapHatsRoutine());
        }

        public void Fight()
        {
            StartCoroutine(FightRoutine());
        }

        public IEnumerator BuildIntentionsRoutine()
        {
            Debug.Log("Start building intentions");

            foreach (Unit unit in GetUnitsInOrder())
            {
                yield return unit.ConjureIntentionRoutine();
            }

            Debug.Log("Done building intentions");

            _fightButton.interactable = true;
        }

        private IEnumerator SwapHatsRoutine()
        {
            Debug.Log("Swapping hats");

            yield return null;
        }

        private IEnumerator FightRoutine()
        {
            _fightButton.interactable = false;

            Debug.Log("Start fighting");

            foreach (Unit unit in GetUnitsInOrder())
            {
                yield return unit.PlayInentionRoutine();
            }

            Debug.Log("Done fighting");

            StartCoroutine(BuildIntentionsRoutine());
        }

        private void MouseClickPerformed(InputAction.CallbackContext obj)
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider == null)
            {
                return;
            }

            Unit clickedUnit = hit.collider.gameObject.GetComponentInParent<Unit>();

            if (clickedUnit == null)
            {
                return;
            }

            StartCoroutine(clickedUnit.PrepareHatSwapRoutine());
        }

        private IEnumerable<Unit> GetUnitsInOrder()
        {
            int maxLength = Mathf.Max(_playerUnits.Length, _enemyUnits.Length);

            for (int i = 0; i < maxLength; i++)
            {
                if (i < _playerUnits.Length && _playerUnits[i].IsAlive())
                {
                    yield return _playerUnits[i];
                }

                if (i < _enemyUnits.Length && _enemyUnits[i].IsAlive())
                {
                    yield return _enemyUnits[i];
                }
            }
        }
    }
}