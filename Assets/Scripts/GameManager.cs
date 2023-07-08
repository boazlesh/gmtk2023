using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Role[] _playerUnits;
        [SerializeField] private Role[] _enemyUnits;

        public void Fight()
        {
            Debug.Log("Fight!");
        }
    }
}