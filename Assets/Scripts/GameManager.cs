using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Role[] PlayerUnits;
        [SerializeField] private Role[] EnemyUnits;

        public void Fight()
        {
            Debug.Log("Fight!");
        }
    }
}