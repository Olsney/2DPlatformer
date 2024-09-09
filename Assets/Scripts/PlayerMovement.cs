using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private const string Horizontal = "Horizontal";

    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _jumpForce = 4f;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private Animator _animator;

    private Rigidbody2D _rigidbody;
    private float _directionX;
    private float _velocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _directionX = Input.GetAxis(Horizontal);
        _velocity = Mathf.Abs(_rigidbody.velocity.x);

        _animator.SetFloat(AnimatorData.Speed, _velocity);
        _animator.SetBool(AnimatorData.IsGrounded, _groundChecker.IsGrounded);
        
            TryFlip();

        if (TryJump())
        {
            Jump();
            _animator.SetBool(AnimatorData.IsGrounded, _groundChecker.IsGrounded);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_directionX * _speed, _rigidbody.velocity.y);
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
        Input.GetKeyDown(KeyCode.Space) && _groundChecker.IsGrounded;
}