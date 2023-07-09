using Assets.Scripts.Enums;
using Assets.Scripts.Mappings;
using Assets.Scripts.Utils;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
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
        [SerializeField] private RoleTable _roleTable;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private InfoDisplayManager _infoDisplayManager;

        public static GameManager Instance { get; private set; }

        private Input _input;
        private Unit _currentSwappingHat;
        private bool _isHatInteractble;
        private Ability _hoveringAbility;
        private bool _isBuildingIntentions;

        [SerializeField] public List<Ability> _bossOffensiveAbilities;
        [SerializeField] public List<Ability> _bossDefensiveAbilities;
        [SerializeField] public List<Ability> _bossSpecialAbilities;

        private void Awake()
        {
            Instance = gameObject.GetComponent<GameManager>();

            _input = new Input();

            _input.Battle.MouseClick.performed += MouseClickPerformed;
            _input.Battle.MouseMove.performed += MouseMovePerformed;

            _input.Enable();
        }

        public void Start()
        {
            IntializeUnits();

            _fightButton.interactable = false;
            _isHatInteractble = true;

            Debug.Log($"Loaded {_playerUnits.Length} players and {_enemyUnits.Length} enemies");

            _roleTable.OnAbilityHovered += OnRoleTableAbilityHovered;
            _roleTable.OnAbilityUnhovered += OnRoleTableAbilityUnhovered;

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
            float swapDuration = 0.5f;

            Vector3 originPosition = originUnit.transform.position;
            Vector3 targetPosition = targetUnit.transform.position;

            originUnit.transform.DOMove(targetPosition, swapDuration);
            targetUnit.transform.DOMove(originPosition, swapDuration);

            yield return new WaitForSeconds(swapDuration);

            Unit[] team = originUnit.GetTeam();
            team[originUnit.UnitIndex] = targetUnit;
            team[targetUnit.UnitIndex] = originUnit;

            int tempIndex = originUnit.UnitIndex;
            originUnit.UnitIndex = targetUnit.UnitIndex;
            targetUnit.UnitIndex = tempIndex;

            // No need - DOMove already did it
            //Vector3 tempPosition = originUnit.transform.position;
            //originUnit.transform.position = targetUnit.transform.position;
            //targetUnit.transform.position = tempPosition;

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
            _isBuildingIntentions = true;
            Debug.Log("Start building intentions");

            foreach (Unit unit in GetUnitsInOrder())
            {
                yield return unit.PlanIntentionRoutine();
            }

            Debug.Log("Done building intentions");

            _isBuildingIntentions = false;
            _fightButton.interactable = true;
        }

        private IEnumerator FightRoutine()
        {
            _fightButton.interactable = false;
            yield return ResetHatSwapRoutine();
            _isHatInteractble = false;

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

            _isHatInteractble = true;

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

        private void MouseMovePerformed(InputAction.CallbackContext obj)
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            Higlight(hit.collider);
        }

        private void Higlight(Collider2D collider)
        {
            if (_hoveringAbility != null)
            {
                return;
            }

            _roleTable.ClearHighlights();

            foreach (Unit unit in _enemyUnits)
            {
                unit.Unhighlight();
            }

            foreach (Unit unit in _playerUnits)
            {
                unit.Unhighlight();
            }

            _infoDisplayManager.Hide();

            if (collider == null)
            {
                return;
            }

            TargetUnitUI targetUI = collider.GetComponent<TargetUnitUI>();

            if (targetUI != null)
            {
                Unit originatingUnit = targetUI.GetComponentInParent<Unit>();

                if (originatingUnit.IsPlayerUnit == targetUI.TargetUnit.IsPlayerUnit)
                {
                    targetUI.TargetUnit.HighlightAlly();
                }
                else
                {
                    targetUI.TargetUnit.HighlightEnemy();
                }
                
                return;
            }

            VerbUI verbHit = collider.GetComponent<VerbUI>();

            if (verbHit != null)
            {
                _roleTable.HighlightVerb(verbHit.Verb);
                return;
            }

            AbilityUI abilityUI = collider.GetComponent<AbilityUI>();

            if (abilityUI != null)
            {
                _roleTable.HighlightAbility(abilityUI.Ability);
                _infoDisplayManager.SetAbility(abilityUI.Ability);
                return;
            }

            Unit hoveredUnit = collider.gameObject.GetComponentInParent<Unit>();

            if (hoveredUnit != null)
            {
                _roleTable.HighlightRole(hoveredUnit.HatRole);

                if (hoveredUnit.IsPlayerUnit)
                {
                    hoveredUnit.HighlightAlly();
                }
                else
                {
                    hoveredUnit.HighlightEnemy();
                }
            }
        }

        private void OnRoleTableAbilityHovered(Ability ability)
        {
            _hoveringAbility = ability;
            _roleTable.HighlightAbility(ability);
            _infoDisplayManager.SetAbility(ability);
        }

        private void OnRoleTableAbilityUnhovered(Ability ability)
        {
            _hoveringAbility = null;
            _roleTable.ClearHighlights();
            _infoDisplayManager.Hide();
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
            if (unit.BodyRole == Role.Boss)
            {
                // TODO: Laugh
                yield break;
            }

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
            yield return new WaitForSeconds(0.2f);

            float swapDuration = 0.7f;

            Vector3 aPosition = unitA.HatContainer.position;
            Vector3 bPosition = unitB.HatContainer.position;

            unitA.HatContainer.DOMove(bPosition, swapDuration);
            unitB.HatContainer.DOMove(aPosition, swapDuration);
            unitA.HatContainer.DORotateQuaternion(unitB.OriginalHatRotation, swapDuration);
            unitB.HatContainer.DORotateQuaternion(unitA.OriginalHatRotation, swapDuration);

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
            _fightButton.interactable = !_isBuildingIntentions;
        }

        private IEnumerable<Unit> GetUnitsInOrder()
        {
            Unit[] playerUnitsClone = _playerUnits.ToArray();
            Unit[] enemyUnitsClone = _enemyUnits.ToArray();

            //int maxLength = Mathf.Max(playerUnitsClone.Length, enemyUnitsClone.Length);

            //for (int i = 0; i < maxLength; i++)
            //{
            //    if (i < playerUnitsClone.Length && playerUnitsClone[i].IsAlive())
            //    {
            //        yield return playerUnitsClone[i];
            //    }

            //    if (i < enemyUnitsClone.Length && enemyUnitsClone[i].IsAlive())
            //    {
            //        yield return enemyUnitsClone[i];
            //    }
            //}

            return playerUnitsClone.Concat(enemyUnitsClone).Where(unit => unit.IsAlive());
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
            _audioSource.FadeOut(0.1f);
        }
    }
}