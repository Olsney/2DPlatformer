using System;
using System.Collections;
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

        private PlayerMover _mover;
        private int _health;
        private Coroutine _attackCoroutine;
        private Rigidbody2D _rigidbody;

        public Transform Transform => transform;

        public void Init()
        {
            _mover = GetComponent<PlayerMover>();
            _health = _config.MaxHealth;
            _rigidbody = GetComponent<Rigidbody2D>();
            _mover.Init(_rigidbody);
        }

        // private void Awake()
        // {
        //     _mover = GetComponent<PlayerMover>();
        //     _health = _config.MaxHealth;
        //     _rigidbody = GetComponent<Rigidbody2D>();
        //     _mover.Init(_rigidbody);
        // }

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
            //У энеми такой же метод атаки и получения урона. Можно ли вынести в класс Attacker и Damagable с базовой реализацией,
            //которые будут наследовать интерфейсы и просто прокинуть их сюда?
            //Или это норм так как уже есть интерфейсы? Тогда просто тут и у врага получится
            //_attacker.Attack(enemy) и _damageable.TakeDamage(damage)

            // if (_enemyChecker.TryGetEnemy(out enemy) == false)
            //     return;

            // enemy.TakeDamage(_config.Damage);
            // _animator.Attack();

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
            if (damage < 0)
                damage = 0;

            _health -= damage;

            Debug.Log($"Игрок получил урон.");
            Debug.Log($"Здоровье игрока: {_health}");

            if (_health <= 0)
            {
                Debug.Log("Игрок погиб");

                Destroy(gameObject);

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void TakeHeal(int heal)
        {
            if (heal < 0)
                throw new Exception("Лечение не может быть отрицательным");

            _health += heal;

            ClampHealth();

            Debug.Log($"Пополнили здоровье. Текущее здоровье: {_health}");
        }

        private void ClampHealth()
        {
            if (_health > _config.MaxHealth)
                _health = _config.MaxHealth;
        }
    }
}