using Services.InputServices;
using UnityEngine;
using World.Characters.Players.Animators;

namespace World.Characters.Players.Systems
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 7f;
        [SerializeField] private float _jumpForce = 4f;
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private PlayerAnimator _playerAnimator;
    
        private Rigidbody2D _rigidbody;
        private float _directionX;
        private float _velocity;
        private IInputService _inputService;

        public void Init(Rigidbody2D rigidbody2d)
        {
            _rigidbody = rigidbody2d;
            _inputService = new StandaloneInputService();
        }

        public void Move()
        {
            _rigidbody.velocity = new Vector2(_directionX * _speed, _rigidbody.velocity.y);
        }

        public void CalculateDirection()
        {
            _directionX = _inputService.DirectionX;
            _velocity = Mathf.Abs(_rigidbody.velocity.x);

            _playerAnimator.PlayMove(_velocity);
            _playerAnimator.TryPlayJump(_groundChecker.IsGrounded);
        
            TryFlip();

            if (TryJump())
            {
                Jump();
                _playerAnimator.TryPlayJump(_groundChecker.IsGrounded);
            }
        }

        private void Jump()
        {
            if (_groundChecker.IsGrounded)
            {
                _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            }
        }

        private void TryFlip()
        {
            float leftRotation = -180f;
        
            if (_directionX > 0)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);

                return;
            }
        
            if (_directionX < 0)
            {
                transform.rotation = Quaternion.Euler(0, leftRotation, 0);
            }
        }

        private bool TryJump() =>
            _inputService.IsJumping && _groundChecker.IsGrounded;
    }
}