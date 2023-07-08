using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Unit[] _playerUnits;
        [SerializeField] private Unit[] _enemyUnits;

        public void Fight()
        {
            Debug.Log("Start fighting");

            foreach (Unit unit in GetUnitsInOrder())
            {
                unit.Play();
            }

            Debug.Log("Done fighting");
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