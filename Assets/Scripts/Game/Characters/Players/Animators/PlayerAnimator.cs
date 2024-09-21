using UnityEngine;
using World.Animators;

namespace World.Characters.Players.Animators
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        public void PlayMove(float velocity) => 
            _animator.SetFloat(AnimatorData.PlayerData.Speed, velocity);

        public void TryPlayJump(bool isGrounded) => 
            _animator.SetBool(AnimatorData.PlayerData.IsGrounded, isGrounded);

        public void Attack() =>
            _animator.SetTrigger(AnimatorData.PlayerData.Attack);
    }
}