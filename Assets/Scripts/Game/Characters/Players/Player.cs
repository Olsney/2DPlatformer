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
                if (_drainHealthDetector.TryGetEnemy(out IDamageable targetToDrain))
                    DrainHealth(targetToDrain);

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

            _drainHealthCoroutine = StartCoroutine(DrainHealthJob(enemy));
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

        public IEnumerator DrainHealthJob(IDamageable enemy)
        {
            float duration = 6f;
            float waitTime = 1f;
            int drainPerIteration = 10;
            var wait = new WaitForSecondsRealtime(waitTime);
            float castProgress = 0;
            float castRadius = 8.5f;

            if (_cooldownDrainHealthCoroutine != null)
                yield break;

            _drainBar.Activate(duration);

            while (duration > 0 && enemy.IsDestroyed == false && IsInCastRadius(enemy, castRadius))
            {
                _drainBar.DisplayProgress(castProgress);

                duration--;
                castProgress++;

                enemy.TakeDamage(drainPerIteration);
                _healthModel.TakeHeal(drainPerIteration);

                yield return wait;
            }

            _cooldownDrainHealthCoroutine = StartCoroutine(LaunchDrainCooldown());
            _drainHealthCoroutine = null;
        }
        
        private IEnumerator AttackJob(IDamageable enemy)
        {
            float delay = 0.5f;
            var wait = new WaitForSeconds(delay);

            while (enabled)
            {
                if (_drainHealthCoroutine != null)
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

        private bool IsInCastRadius(IDamageable enemy, float castRadius) =>
            Vector3.Distance(transform.position, enemy.Position) < castRadius;

        private IEnumerator LaunchDrainCooldown()
        {
            float cooldown = 6f;
            float time = 1f;

            var wait = new WaitForSecondsRealtime(time);

            _drainBar.DisplayProgress(cooldown);

            while (cooldown > 0)
            {
                cooldown--;
                _drainBar.DisplayProgress(cooldown);

                yield return wait;
            }

            _drainBar.Disable();

            _cooldownDrainHealthCoroutine = null;
        }
    }
}