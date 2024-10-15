using System.Collections;
using Game.UI;
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
        [SerializeField] private AttackAbilityFinder attackAbilityFinder;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private EnemyConfig _config;
        [SerializeField] private EnemyMover _mover;
        [SerializeField] private HealthModel _healthModel;
        [SerializeField] private HealthPresenter _healthPresenter;

        private Coroutine _currentCoroutine;

        public Vector3 Position => transform.position;
        public bool IsDestroyed { get; private set; }

        public bool IsTransformNull => transform == null;

        public void Init()
        {
            _mover.Init();
            _healthModel.Init(_config.MaxHealth);
            _healthPresenter.Init();
            _mover.MoveToPoint();
        }
        private void OnEnable()
        {
            _playerFinder.Found += _mover.MoveToPlayer;
            _playerFinder.Lost += _mover.MoveToPoint;
            attackAbilityFinder.Found += Attack;
            attackAbilityFinder.Lost += StopAttack;
        }

        private void OnDisable()
        {
            _playerFinder.Found -= _mover.MoveToPlayer;
            _playerFinder.Lost -= _mover.MoveToPoint;
            attackAbilityFinder.Found -= Attack;
            attackAbilityFinder.Lost -= StopAttack;
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

        public void TakeDamage(int damage)
        {
            _healthModel.TakeDamage(damage);
        
            if (_healthModel.Value <= 0)
            {
                Debug.Log("Враг умер");

                IsDestroyed = true;
            
                Destroy(gameObject);
            }
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
    }
}