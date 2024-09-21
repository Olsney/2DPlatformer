using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using World.Characters.Enemies.Animators;
using World.Characters.Enemies.Configs;
using World.Characters.Enemies.Systems;
using World.Characters.Interfaces;

namespace World.Characters.Enemies
{
    public class Enemy : MonoBehaviour, IAttacker, IDamageable
    {
        [FormerlySerializedAs("_playerChecker")] [SerializeField] private PlayerFinder playerFinder;
        [FormerlySerializedAs("_attackChecker")] [SerializeField] private AttackAbility attackAbility;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private EnemyConfig _config;

        private int _health;
        private EnemyMover _mover;
        private Coroutine _currentCoroutine;

        public void Init()
        {
            _mover = GetComponent<EnemyMover>();
            _mover.Init();

            _health = _config.MaxHealth;
        }

        // private void Awake()
        // {
        //     _mover = GetComponent<EnemyMover>();
        //     _mover.Init();
        //
        //     _health = _config.MaxHealth;
        // }

        private void OnEnable()
        {
            playerFinder.Found += _mover.MoveTo;
            playerFinder.Lost += _mover.MoveToPoint;
            attackAbility.Founded += Attack;
            attackAbility.Lost += StopAttack;
        }

        private void OnDisable()
        {
            playerFinder.Found -= _mover.MoveTo;
            playerFinder.Lost -= _mover.MoveToPoint;
            attackAbility.Founded -= Attack;
            attackAbility.Lost -= StopAttack;
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