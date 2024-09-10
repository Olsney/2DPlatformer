using UnityEngine;

namespace Animators
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        public void PlayMove(float velocity) => 
            _animator.SetFloat(AnimatorData.PlayerData.Speed, velocity);

        public void PlayJump(bool isGrounded) => 
            _animator.SetBool(AnimatorData.PlayerData.IsGrounded, isGrounded);
    }
}