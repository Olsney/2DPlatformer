using System.Collections;
using UnityEngine;
using World.Characters.Enemies.Animators;
using World.Characters.Enemies.Configs;
using World.Characters.Enemies.Systems;
using World.Characters.Interfaces;

namespace World.Characters.Enemies
{
    public class Enemy : MonoBehaviour, IAttacker, IDamageable
    {
        [SerializeField] private PlayerFinder _playerFinder;
        [SerializeField] private AttackAbility _attackAbility;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private EnemyConfig _config;
        [SerializeField] private EnemyMover _mover;

        private int _health;
        private Coroutine _currentCoroutine;

        public void Init()
        {
            _mover.Init();
        
            _health = _config.MaxHealth;
        }

        private void OnEnable()
        {
            _playerFinder.Found += _mover.MoveToPlayer;
            _playerFinder.Lost += _mover.MoveToPoint;
            _attackAbility.Founded += Attack;
            _attackAbility.Lost += StopAttack;
        }

        private void OnDisable()
        {
            _playerFinder.Found -= _mover.MoveToPlayer;
            _playerFinder.Lost -= _mover.MoveToPoint;
            _attackAbility.Founded -= Attack;
            _attackAbility.Lost -= StopAttack;
        }
    
        public void Attack(IDamageable player)
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
        
            _currentCoroutine = StartCoroutine(AttackJob(player));
        }

        private void StopAttack()
        {
            if (_currentCoroutine == null)
                return;
        
            StopCoroutine(_currentCoroutine);
        }

        private IEnumerator AttackJob(IDamageable player)
        {
            float delay = 1f;
            var wait = new WaitForSeconds(delay);
            
            while (enabled)
            {
                player.TakeDamage(_config.Damage);
                _animator.Attack();
            
                yield return wait;
            }
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                damage = 0;

            _health -= damage;
        
            Debug.Log($"Враг получил урон.");
            Debug.Log($"Здоровье врага: {_health}");
        
            if (_health <= 0)
            {
                Debug.Log("Враг умер");
            
                Destroy(gameObject);
            }
        }
    }
}