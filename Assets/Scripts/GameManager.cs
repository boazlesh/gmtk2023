using Assets.Scripts.Enums;
using DG.Tweening;
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

        public static GameManager Instance { get; private set; }

        private Input _input;
        private Unit _currentSwappingHat;

        private void Awake()
        {
            Instance = gameObject.GetComponent<GameManager>();

            _input = new Input();

            _input.Battle.MouseClick.performed += MouseClickPerformed;

            _input.Enable();
        }

        public void Start()
        {
            SetIsPlayerUnits();

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

        public Unit GetUnitByIntention(Intention intention)
        {
            Unit[] units = intention.Originator.GetVerbPossibleTargetTeam(intention.Verb);
            return units[intention.TargetIndex];
        }

        private IEnumerator BuildIntentionsRoutine()
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
                StartCoroutine(ResetHatSwapRoutine());
                return;
            }

            StartCoroutine(UnitClickedRoutine(clickedUnit));
        }

        private IEnumerator ResetHatSwapRoutine()
        {
            if (_currentSwappingHat == null)
            {
                yield break;
            }

            yield return _currentSwappingHat.UnfloatHatRoutine();
            _currentSwappingHat = null;
        }

        private IEnumerator UnitClickedRoutine(Unit unit)
        {
            if (_currentSwappingHat == null)
            {
                _currentSwappingHat = unit;

                yield return unit.FloatHatRoutine();
                yield break;
            }

            yield return SwapHatsRoutine(_currentSwappingHat, unit);
            _currentSwappingHat = null;
        }

        private IEnumerator SwapHatsRoutine(Unit unitA, Unit unitB)
        {
            Debug.Log($"Swapping hats - {unitA.name} x {unitB.name}");

            yield return unitB.FloatHatRoutine();
            yield return new WaitForSeconds(0.5f);

            float swapDuration = 1f;

            Vector3 aPosition = unitA.HatSpriteRenderer.transform.position;
            Vector3 bPosition = unitB.HatSpriteRenderer.transform.position;

            unitA.HatSpriteRenderer.transform.DOMove(bPosition, swapDuration);
            unitB.HatSpriteRenderer.transform.DOMove(aPosition, swapDuration);

            yield return new WaitForSeconds(swapDuration);

            Sprite spriteA = unitA.HatSpriteRenderer.sprite;
            unitA.HatSpriteRenderer.sprite = unitB.HatSpriteRenderer.sprite;
            unitB.HatSpriteRenderer.sprite = spriteA;

            Role roleA = unitA.HatRole;
            unitA.HatRole = unitB.HatRole;
            unitB.HatRole = roleA;

            yield return this.WhenAllRoutine(unitA.UnfloatHatRoutine(), unitB.UnfloatHatRoutine());
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

        private void SetIsPlayerUnits()
        {
            foreach (Unit unit in _playerUnits)
            {
                unit._isPlayerUnit = true;
            }
        }
    }
}