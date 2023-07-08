using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public Unit[] _playerUnits;
        [SerializeField] public Unit[] _enemyUnits;
        [SerializeField] private Button _fightButton;

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