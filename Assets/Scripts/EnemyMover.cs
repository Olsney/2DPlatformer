using System.Collections;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private Transform[] _points;

    private int _currentPointIndex;
    private Transform _currentTarget;
    private Coroutine _currentCoroutine;

    private void Awake()
    {
        _currentPointIndex = 0;
        _currentTarget = _points[_currentPointIndex];
    }

    private void Start()
    {
        TryFlip(_currentTarget);
    }

    private void Update()
    {
        // MoveToPoint();
        //
        // if (IsOnPoint())
        // {
        //     ChangePoint();
        //     TryFlip();
        // }
    }

    public void MoveToPlayer(Transform playerTransform)
    {
        // if (playerTransform == null)
        //     return;
        //
        // transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, _speed * Time.deltaTime);

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }

        _currentCoroutine = StartCoroutine(MoveToPlayerJob(playerTransform));
    }

    public void TryFlip(Transform followedTransform)
    {
        float leftRotation = -180f;

        // if (transform.position.x > _currentTarget.position.x)
        if (transform.position.x > followedTransform.position.x)
            transform.rotation = Quaternion.Euler(0, -leftRotation, 0);
        else
            transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void MoveToPoint()
    {
        // transform.position = Vector2.MoveTowards(transform.position, _currentTarget.position, _speed * Time.deltaTime);

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }

        TryFlip(_currentTarget);

        _currentCoroutine = StartCoroutine(MoveToPointJob());
    }

    public void ChangePoint()
    {
        _currentPointIndex++;

        if (_currentPointIndex >= _points.Length)
            _currentPointIndex = 0;

        _currentTarget = _points[_currentPointIndex];
    }

    public bool IsDistanceReached(Transform followedTransform)
    {
        float permissibleDifference = 1f;

        return (transform.position - followedTransform.position).sqrMagnitude < permissibleDifference;
    }

    private IEnumerator MoveToPointJob()
    {
        while (enabled)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, _currentTarget.position, _speed * Time.deltaTime);

            if (IsDistanceReached(_currentTarget))
            {
                ChangePoint();
                TryFlip(_currentTarget);
            }

            yield return null;
        }
    }

    private IEnumerator MoveToPlayerJob(Transform playerTransform)
    {
        while (enabled)
        {
            Debug.Log($"Бегу по трансформу к игроку {playerTransform}");

            if (playerTransform == null)
                yield return null;

            if (IsDistanceReached(playerTransform) == false)
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, playerTransform.position, _speed * Time.deltaTime);
            }
            
            Debug.Log($"Перестал бежать за игроком");
            
            TryFlip(playerTransform);

            yield return null;
        }
    }
}