using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private Transform _path;

    private Rigidbody2D _rigidbody;
    private bool _isLookingRight;
    private Transform[] _points;
    private int _currentPointIndex = 0;
    private Transform _currentTarget;

    private void Awake()
    {
        _isLookingRight = true;
        _points = new Transform[_path.childCount];

        for (int i = 0; i < _path.childCount; i++)
            _points[i] = _path.GetChild(i);

        _rigidbody = GetComponent<Rigidbody2D>();

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
        if (transform.position.x > _currentTarget.position.x)
            transform.rotation = Quaternion.Euler(0, -180f, 0);
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

    private bool IsOnPoint() =>
        (transform.position - _currentTarget.position).magnitude < 1f;
}