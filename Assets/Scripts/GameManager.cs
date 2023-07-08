using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Role[] PlayerUnits;
        public Role[] EnemyUnits;

        public void Fight()
        {
            Debug.Log("Fight!");
        }
    }
}