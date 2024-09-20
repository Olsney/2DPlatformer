using System.Collections;
using Animators;
using Services.InputServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _jumpForce = 4f;
    [SerializeField] private GroundChecker _groundChecker;
    // [SerializeField] private Animator _animator;
    [SerializeField] private PlayerAnimator _playerAnimator;
    
    private Rigidbody2D _rigidbody;
    private float _directionX;
    private float _velocity;
    private IInputService _inputService;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
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
        // _animator.SetFloat(AnimatorData.PlayerData.Speed, _velocity);
        // _animator.SetBool(AnimatorData.PlayerData.IsGrounded, _groundChecker.IsGrounded);
        
        TryFlip();

        if (TryJump())
        {
            Jump();
            _playerAnimator.TryPlayJump(_groundChecker.IsGrounded);
            // _animator.SetBool(AnimatorData.PlayerData.IsGrounded, _groundChecker.IsGrounded);
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