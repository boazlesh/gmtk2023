using Assets.Scripts.Enums;
using Assets.Scripts.Mappings;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public Unit[] _playerUnits;
        [SerializeField] public Unit[] _enemyUnits;
        [SerializeField] public VerbPerRoleMapping _verbPerRoleMapping;
        [SerializeField] private Button _fightButton;
        [SerializeField] private WinWindow _winWindowPrefab;
        [SerializeField] private LoseWindow _loseWindowPrefab;
        [SerializeField] public ModifiedHealthNumberIndicator _modifiedHealthNumberIndicatorPrefab;

        public static GameManager Instance { get; private set; }

        private Input _input;
        private Unit _currentSwappingHat;
        private bool _isHatInteractble;

        private void Awake()
        {
            Instance = gameObject.GetComponent<GameManager>();

            _input = new Input();

            _input.Battle.MouseClick.performed += MouseClickPerformed;

            _input.Enable();
        }

        public void Start()
        {
            IntializeUnits();

            _fightButton.interactable = false;
            _isHatInteractble = false;

            Debug.Log($"Loaded {_playerUnits.Length} players and {_enemyUnits.Length} enemies");


            BuildIntentions();
        }

        public void BuildIntentions()
        {
            StartCoroutine(BuildIntentionsRoutine());
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

        public IEnumerator SwapUnitsRoutine(Unit originUnit, Unit targetUnit)
        {
            Unit[] team = originUnit.GetTeam();
            team[originUnit.UnitIndex] = targetUnit;
            team[targetUnit.UnitIndex] = originUnit;

            int tempIndex = originUnit.UnitIndex;
            originUnit.UnitIndex = targetUnit.UnitIndex;
            targetUnit.UnitIndex = tempIndex;

            Vector3 tempPosition = originUnit.transform.position;
            originUnit.transform.position = targetUnit.transform.position;
            targetUnit.transform.position = tempPosition;

            foreach (Unit unit in _playerUnits.Concat(_enemyUnits))
            {
                unit.InvalidateIntention();
            }

            yield return null;
        }

        public void CheckWinLose()
        {
            if (_enemyUnits.All(unity => !unity.IsAlive()))
            {
                Win();

                return;
            }

            if (_playerUnits.All(unity => !unity.IsAlive()))
            {
                Lose();

                return;
            }
        }

        private IEnumerator BuildIntentionsRoutine()
        {
            Debug.Log("Start building intentions");

            foreach (Unit unit in GetUnitsInOrder())
            {
                yield return unit.PlanIntentionRoutine();
            }

            Debug.Log("Done building intentions");

            _fightButton.interactable = true;
            _isHatInteractble = true;
        }

        private IEnumerator FightRoutine()
        {
            _fightButton.interactable = false;
            ResetHatSwapRoutine();

            Debug.Log("Start fighting");

            foreach (Unit unit in GetUnitsInOrder())
            {
                yield return unit.PlayInentionRoutine();
            }

            Debug.Log("Done fighting");

            foreach (Unit unit in _playerUnits.Concat(_enemyUnits))
            {
                unit.EndTurn();
            }

            StartCoroutine(BuildIntentionsRoutine());
        }

        private void MouseClickPerformed(InputAction.CallbackContext obj)
        {
            if (!_isHatInteractble)
            {
                return;
            }

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

            if (_currentSwappingHat == unit)
            {
                yield return ResetHatSwapRoutine();
                yield break;
            }

            yield return SwapHatsRoutine(_currentSwappingHat, unit);
            _currentSwappingHat = null;
        }

        private IEnumerator SwapHatsRoutine(Unit unitA, Unit unitB)
        {
            Debug.Log($"Swapping hats - {unitA.name} x {unitB.name}");

            _isHatInteractble = false;
            _fightButton.interactable = false;

            yield return unitB.FloatHatRoutine();
            yield return new WaitForSeconds(0.5f);

            float swapDuration = 1f;

            Vector3 aPosition = unitA.HatContainer.position;
            Vector3 bPosition = unitB.HatContainer.position;

            unitA.HatContainer.DOMove(bPosition, swapDuration);
            unitB.HatContainer.DOMove(aPosition, swapDuration);

            bool shouldFlip = unitA.IsPlayerUnit != unitB.IsPlayerUnit;

            if (shouldFlip)
            {
                unitA.HatContainer.DOScaleX(-unitA.HatContainer.localScale.x, swapDuration);
                unitB.HatContainer.DOScaleX(-unitB.HatContainer.localScale.x, swapDuration);
            }

            yield return new WaitForSeconds(swapDuration);

            var unfloatDuration = 0.1f;

            unitA.HatContainer.DOMove(unitB.OriginalHatPosition, unfloatDuration);
            unitB.HatContainer.DOMove(unitA.OriginalHatPosition, unfloatDuration);

            yield return new WaitForSeconds(unfloatDuration);

            if (shouldFlip)
            {
                unitA.HatContainer.DOScaleX(-unitA.HatContainer.localScale.x, 0);
                unitB.HatContainer.DOScaleX(-unitB.HatContainer.localScale.x, 0);
            }

            Sprite spriteA = unitA.HatSpriteRenderer.sprite;
            unitA.HatSpriteRenderer.sprite = unitB.HatSpriteRenderer.sprite;
            unitB.HatSpriteRenderer.sprite = spriteA;

            Role roleA = unitA.HatRole;
            unitA.HatRole = unitB.HatRole;
            unitB.HatRole = roleA;

            yield return this.WhenAllRoutine(unitA.UnfloatHatRoutine(), unitB.UnfloatHatRoutine());

            unitA.InvalidateIntention();
            unitB.InvalidateIntention();

            _isHatInteractble = true;
            _fightButton.interactable = true;
        }

        private IEnumerable<Unit> GetUnitsInOrder()
        {
            Unit[] playerUnitsClone = _playerUnits.ToArray();
            Unit[] enemyUnitsClone = _enemyUnits.ToArray();

            int maxLength = Mathf.Max(playerUnitsClone.Length, enemyUnitsClone.Length);

            for (int i = 0; i < maxLength; i++)
            {
                if (i < playerUnitsClone.Length && playerUnitsClone[i].IsAlive())
                {
                    yield return playerUnitsClone[i];
                }

                if (i < enemyUnitsClone.Length && enemyUnitsClone[i].IsAlive())
                {
                    yield return enemyUnitsClone[i];
                }
            }
        }

        private void IntializeUnits()
        {
            for (int i = 0; i < _playerUnits.Length; i++)
            {
                _playerUnits[i].UnitIndex = i;
                _playerUnits[i].IsPlayerUnit = true;
            }
            for (int i = 0; i < _enemyUnits.Length; i++)
            {
                _enemyUnits[i].UnitIndex = i;
            }
        }

        private void Win()
        {
            Stop();
            Instantiate(_winWindowPrefab);
        }

        private void Lose()
        {
            Stop();
            Instantiate(_loseWindowPrefab);
        }

        private void Stop()
        {
            StopAllCoroutines();

            foreach (Unit unit in _playerUnits.Concat(_enemyUnits))
            {
                unit.StopAllCoroutines();
            }

            _fightButton.interactable = false;
            _isHatInteractble = false;
        }
    }
}