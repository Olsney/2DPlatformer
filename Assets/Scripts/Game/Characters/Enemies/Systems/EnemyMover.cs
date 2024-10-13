using System.Collections;
using UnityEngine;
using World.Characters.Interfaces;

namespace World.Characters.Enemies.Systems
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField] private Transform[] _points;

        private int _currentPointIndex;
        private Transform _currentTarget;
        private Coroutine _currentCoroutine;

        public void Init()
        {
            _currentPointIndex = 0;
            _currentTarget = _points[_currentPointIndex];
            TryFlip(_currentTarget);
        }

        public void MoveToPlayer(IEnemyTarget player)
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }

            _currentCoroutine = StartCoroutine(MoveToPlayerJob(player.Transform));
        }

        public void TryFlip(Transform followedTransform)
        {
            float leftRotation = -180f;

            if (transform.position.x > followedTransform.position.x)
                transform.rotation = Quaternion.Euler(0, -leftRotation, 0);
            else
                transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        public void MoveToPoint()
        {
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
                if (playerTransform == null)
                    yield return null;

                if (IsDistanceReached(playerTransform) == false)
                {
                    transform.position =
                        Vector2.MoveTowards(transform.position, playerTransform.position, _speed * Time.deltaTime);
                }
                
                TryFlip(playerTransform);

                yield return null;
            }
        }
    }
}