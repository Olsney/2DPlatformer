using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private Transform[] _points;
    
    private int _currentPointIndex;
    private Transform _currentTarget;

    private void Awake()
    {
        _currentPointIndex = 0;
        _currentTarget = _points[_currentPointIndex];
    }

    private void Start()
    {
        TryFlip();
    }

    private void Update()
    {
        Move();
        
        if (IsOnPoint())
        {
            ChangePoint();
            TryFlip();
        }
    }

    private void TryFlip()
    {
        float leftRotation = -180f;
        
        if (transform.position.x > _currentTarget.position.x)
            transform.rotation = Quaternion.Euler(0, -leftRotation, 0);
        else
            transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, _currentTarget.position, _speed * Time.deltaTime);
    }

    private void ChangePoint()
    {
        _currentPointIndex++;

        if (_currentPointIndex >= _points.Length)
            _currentPointIndex = 0;
        
        _currentTarget = _points[_currentPointIndex];
    }

    private bool IsOnPoint()
    {
        float epsilon = 1f;
        
        return (transform.position - _currentTarget.position).sqrMagnitude < epsilon;
    }
}