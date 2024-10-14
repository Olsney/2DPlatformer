using System;
using System.Collections;
using System.Timers;
using Game.UI;
using Services.InputServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
        [SerializeField] private DrainBar _drainBar;

        private PlayerMover _mover;
        private Coroutine _attackCoroutine;
        private Rigidbody2D _rigidbody;
        private IInputService _inputService;

        private Coroutine _drainHealthCoroutine;
        private Coroutine _cooldownDrainHealthCoroutine;
        private IDamageable _drainedEnemy;

        public Vector3 Position => transform.position;
        public bool IsDestroyed { get; private set; }

        public Transform Transform => transform;

        public void Init()
        {
            _mover = GetComponent<PlayerMover>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _mover.Init(_rigidbody);
            _healthModel.Init(_config.MaxHealth);
            _healthPresenter.Init();
            _inputService = new StandaloneInputService();
            _drainBar.Disable();

            Debug.Log(_healthModel.Value);
        }

        private void Update()
        {
            _mover.CalculateDirection();

            if (_inputService.IsDrainHealthUsed && _drainHealthCoroutine == null)
            {
                Debug.Log("Кнопка нажата и корутина не запущена");
                
                if (_drainHealthDetector.TryGetEnemy(out IDamageable targetToDrain))
                {
                    Debug.Log("получили врага для дрейна");
                    DrainHealth(targetToDrain);
                }
            }

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

        public void Attack(IDamageable enemy)
        {
            if (_attackCoroutine != null)
                return;

            if (_drainHealthCoroutine != null)
                return;

            _attackCoroutine = StartCoroutine(AttackJob(enemy));
        }

        public void DrainHealth(IDamageable enemy)
        {
            if (_drainHealthCoroutine != null)
                return;

            Debug.Log("Запускаем корутину");

            _drainHealthCoroutine = StartCoroutine(DrainHealthJob(enemy));
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

                IsDestroyed = true;

                Destroy(gameObject);

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void TakeHeal(int heal) =>
            _healthModel.TakeHeal(heal);

        void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, 8.5f);
        }

        public IEnumerator DrainHealthJob(IDamageable enemy)
        {
            int duration = 6;
            int waitTime = 1;
            int drainPerIteration = 10;
            var wait = new WaitForSecondsRealtime(waitTime);
            int castProgress = 0;
            float castRadius = 8.5f;

            if (_cooldownDrainHealthCoroutine != null)
            {
                Debug.Log("Висит кулдаун на корутине");
                
                yield break;
            }

            _drainBar.Activate(duration);

            // _drainedEnemy = enemy;
            // _drainHealthDetector.Lost += OnDrainLost;

            while (duration > 0 && enemy.IsDestroyed == false && Vector3.Distance(transform.position, enemy.Position) < castRadius)
            {
                _drainBar.DisplayProgress(castProgress);
                
                duration--;
                castProgress++;

                Debug.Log("Мы запустили корутину и в цикле вайл");

                // gameObject.IsDestroyed();
                

                // if (Vector3.Distance(transform.position, enemy.Position) > castRadius)
                // {
                //     _cooldownDrainHealthCoroutine = StartCoroutine(LaunchDrainCooldown());
                //     _drainHealthCoroutine = null;
                //     
                //     Debug.Log("Враг убежал, останавливаем корутину.");
                //     
                //     yield break;
                // }

                
                enemy.TakeDamage(drainPerIteration);
                _healthModel.TakeHeal(drainPerIteration);
                
                Debug.Log($"Мы нанесли и постарались поглотить {drainPerIteration}");

                yield return wait;
            }

            _drainBar.Disable();
            _drainHealthCoroutine = null;
            _cooldownDrainHealthCoroutine = StartCoroutine(LaunchDrainCooldown());
            Debug.Log("Запустили кулдаун.");
            
        }

        private IEnumerator LaunchDrainCooldown()
        {
            int cooldown = 6;
            float time = 1f;

            var wait = new WaitForSecondsRealtime(time);
            
            _drainBar.Activate(cooldown);

            while (cooldown > 0)
            {
                _drainBar.DisplayProgress(cooldown);
                cooldown--;
                
                yield return wait;
            }

            _drainBar.Disable();

            _cooldownDrainHealthCoroutine = null;
        }
    }
}