using UnityEngine;
using World.Characters.Enemies;
using World.Characters.Interfaces;

namespace World.Characters.Players.Systems
{
    public class EnemyChecker : MonoBehaviour
    {
        private Enemy _currentEnemy;

        private bool _isEnemyFounded;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                _currentEnemy = enemy;
                _isEnemyFounded = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                _currentEnemy = null;
                _isEnemyFounded = false;
            }
        }
    
        public bool TryGetEnemy(out IDamageable enemy)
        {
            enemy = _currentEnemy;

            if (_isEnemyFounded == false)
                return false;

            return true;
        }
    }
}