using System;
using UnityEngine;
using World.Characters.Enemies;
using World.Characters.Interfaces;

namespace World.Characters.Players.Systems
{
    public class EnemyDetector : MonoBehaviour
    {
        public event Action<Collider2D> Lost;
            
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

            Lost?.Invoke(collider);
        }
    
        public bool TryGetEnemy(out IDamageable enemy)
        {
            enemy = _currentEnemy;

            if (_isEnemyFounded == false)
                return false;

            return true;
        }

        // public bool TryGetDrainedEnemy(IDamageable enemy) =>
        //     enemy == _currentEnemy;
    }
}