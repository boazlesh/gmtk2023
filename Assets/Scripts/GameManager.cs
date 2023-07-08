using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Unit[] _playerUnits;
        [SerializeField] private Unit[] _enemyUnits;
        [SerializeField] private Button _fightButton;

        public void Fight()
        {
            StartCoroutine(FightRoutine());
        }

        private IEnumerator FightRoutine()
        {
            _fightButton.interactable = false;

            Debug.Log("Start fighting");

            foreach (Unit unit in GetUnitsInOrder())
            {
                yield return unit.PlayRoutine();
            }

            Debug.Log("Done fighting");

            _fightButton.interactable = true;
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