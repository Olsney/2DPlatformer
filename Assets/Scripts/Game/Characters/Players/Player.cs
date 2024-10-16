using System.Collections;
using Game.UI;
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
        [SerializeField] private EnemyChecker _enemyChecker;
        [SerializeField] private PlayerConfig _config;
        [SerializeField] private HealthModel _healthModel;
        [SerializeField] private HealthPresenter _healthPresenter;

        private PlayerMover _mover;
        private Coroutine _attackCoroutine;
        private Rigidbody2D _rigidbody;

        public Transform Transform => transform;

        public void Init()
        {
            _mover = GetComponent<PlayerMover>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _mover.Init(_rigidbody);
            _healthModel.Init(_config.MaxHealth);
            _healthPresenter.Init();

            Debug.Log(_healthModel.Value);
        }

        private void Update()
        {
            _mover.CalculateDirection();

            if (_enemyChecker.TryGetEnemy(out IDamageable enemy) == false)
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

        public void Attack(IDamageable enemy)
        {
            if (_attackCoroutine != null)
                return;

            _attackCoroutine = StartCoroutine(AttackJob(enemy));
        }

        private IEnumerator AttackJob(IDamageable enemy)
        {
            float delay = 0.5f;
            var wait = new WaitForSeconds(delay);

            while (enabled)
            {
                enemy.TakeDamage(_config.Damage);
                _animator.Attack();

                yield return wait;
            }
        }

        public void TakeDamage(int damage)
        {
            _healthModel.TakeDamage(damage);

            if (_healthModel.Value <= 0)
            {
                Debug.Log("Игрок погиб");

                Destroy(gameObject);

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void TakeHeal(int heal) => 
            _healthModel.TakeHeal(heal);
    }
}