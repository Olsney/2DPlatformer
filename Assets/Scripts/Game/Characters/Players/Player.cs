using System.Collections;
using Game.UI;
using Services.InputServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using World.Characters.Interfaces;
using World.Characters.Players.Animators;
using World.Characters.Players.Configs;
using World.Characters.Players.Systems;
using World.Environment;

namespace World.Characters.Players
{
    [RequireComponent(typeof(PlayerMover), typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IAttacker, IDamageable, IHealeable, IEnemyTarget
    {
        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private PlayerConfig _config;
        [SerializeField] private HealthModel _healthModel;
        [SerializeField] private HealthPresenter _healthPresenter;
        [SerializeField] private EnemyDetector _attackDetector;
        [SerializeField] private EnemyDetector _drainHealthDetector;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private DrainHealthAbility _drainHealthAbility;
        
        private PlayerMover _mover;
        private Coroutine _attackCoroutine;
        private Rigidbody2D _rigidbody;
        private IInputService _inputService;

        public Vector3 Position => transform.position;
        public int Health => _healthModel.Value;
        public bool IsDestroyed { get; private set; }

        public Transform Transform => transform;

        private void Update()
        {
            _mover.CalculateDirection();

            if (_inputService.IsDrainHealthUsed && _drainHealthAbility.IsDraining == false)
                if (_drainHealthDetector.TryGetEnemy(out IDamageable targetToDrain))
                    _drainHealthAbility.DrainHealth(targetToDrain);

            if (_attackDetector.TryGetEnemy(out IDamageable enemy) == false)
            {
                if (_attackCoroutine != null)
                {
                    StopCoroutine(_attackCoroutine);
                    _attackCoroutine = null;
                }

                return;
            }

            Attack(enemy);
        }

        private void FixedUpdate()
        {
            _mover.Move();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Coin coin))
                Destroy(coin.gameObject);

            if (collider.TryGetComponent(out HealingKit healingKit))
            {
                healingKit.Heal(this);
                Destroy(healingKit.gameObject);
            }
        }
        
        public void Init()
        {
            _mover = GetComponent<PlayerMover>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _mover.Init(_rigidbody);
            _healthModel.Init(_config.MaxHealth);
            _healthPresenter.Init();
            _inputService = new StandaloneInputService();
            _drainHealthAbility.Init();
        }

        public void Attack(IDamageable enemy)
        {
            if (_attackCoroutine != null)
                return;

            if (_drainHealthAbility.IsDraining)
                return;

            _attackCoroutine = StartCoroutine(AttackJob(enemy));
        }

        public void TakeDamage(int damage)
        {
            _healthModel.TakeDamage(damage);

            if (_healthModel.Value <= 0)
            {
                IsDestroyed = true;

                Destroy(gameObject);

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void TakeHeal(int heal) =>
            _healthModel.TakeHeal(heal);
        
        private IEnumerator AttackJob(IDamageable enemy)
        {
            float delay = 0.5f;
            var wait = new WaitForSeconds(delay);

            while (enabled)
            {
                if (_drainHealthAbility.IsDraining)
                {
                    _attackCoroutine = null;
                    break;
                }
                
                enemy.TakeDamage(_config.Damage);
                _animator.Attack();

                yield return wait;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, 8.5f);
        }
    }
}