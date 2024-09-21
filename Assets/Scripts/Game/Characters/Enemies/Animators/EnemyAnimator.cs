using UnityEngine;
using World.Animators;

namespace World.Characters.Enemies.Animators
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void Attack() =>
            _animator.SetTrigger(AnimatorData.EnemyData.Attack);
    }
}