using System;
using System.Collections;
using UnityEngine;
using World.Characters.Interfaces;

namespace World.Characters.Players.Systems
{
    public class DrainHealthAbility : MonoBehaviour
    {
        [SerializeField] private EnemyDetector _drainAbilityZone;
        [SerializeField] private HealthModel _healthModel;
        [SerializeField] private DrainBar _drainBar;
        
        private Coroutine _drainHealthCoroutine;
        private Coroutine _cooldownDrainHealthCoroutine;
        private IDamageable _drainedEnemy;
        
        public event Action<float> SkillActivated;
        public event Action<float> ProgressChanged;
        public event Action CooldownEnded;
        
        public bool IsDraining => _drainHealthCoroutine != null;

        public void Init()
        {
            _drainBar.Disable();
        }
        
        private void OnEnable()
        {
            _drainBar.Init();
        }

        private void OnDisable()
        {
            _drainBar.OnDispose();
        }

        public void DrainHealth(IDamageable enemy)
        {
            if (_drainHealthCoroutine != null)
                return;

            _drainHealthCoroutine = StartCoroutine(DrainHealthJob(enemy));
        }
        
        private IEnumerator DrainHealthJob(IDamageable enemy)
        {
            float duration = 6f;
            float waitTime = 1f;
            int drainPerIteration = 50;
            var wait = new WaitForSecondsRealtime(waitTime);
            float castProgress = 6;
            float castRadius = 8.5f;

            if (_cooldownDrainHealthCoroutine != null)
                yield break;

            // _drainBar.Activate(duration);
            SkillActivated?.Invoke(duration);

            while (duration > 0 && enemy.IsDestroyed == false && IsInCastRadius(enemy, castRadius))
            {
                // _drainBar.DisplayProgress(castProgress);
                ProgressChanged?.Invoke(castProgress);

                castProgress--;
                duration--;
                
                if (enemy.Health < drainPerIteration)
                    drainPerIteration = enemy.Health;

                enemy.TakeDamage(drainPerIteration);
                _healthModel.TakeHeal(drainPerIteration);

                yield return wait;
            }

            _cooldownDrainHealthCoroutine = StartCoroutine(LaunchDrainCooldown());
            _drainHealthCoroutine = null;
        }
        
        private IEnumerator LaunchDrainCooldown()
        {
            float recoveryProgress = 0;
            float cooldown = 6f;
            float time = 1f;

            var wait = new WaitForSecondsRealtime(time);

            // _drainBar.DisplayProgress(recoveryProgress);
            ProgressChanged?.Invoke(recoveryProgress);

            while (recoveryProgress <= cooldown)
            {
                // _drainBar.DisplayProgress(recoveryProgress);
                ProgressChanged?.Invoke(recoveryProgress);
                recoveryProgress++;

                yield return wait;
            }

            CooldownEnded?.Invoke();
            _drainBar.Disable();

            _cooldownDrainHealthCoroutine = null;
        }
        
        private bool IsInCastRadius(IDamageable enemy, float castRadius) =>
            Vector3.Distance(transform.position, enemy.Position) < castRadius;
    }
}