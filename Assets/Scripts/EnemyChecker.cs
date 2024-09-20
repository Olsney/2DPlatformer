using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    [SerializeField] private float _checkRadius = 1f;
    [SerializeField] private LayerMask _layerMask;

    private Coroutine _checkEnemyJob;
    private Enemy _currentEnemy;
    
    private void Start()
    {
        _checkEnemyJob = StartCoroutine(FindEnemyJob());
    }

    private Enemy TryFindCollision()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, _checkRadius, _layerMask);

        if (collider == null)
            return null;
        
        collider.TryGetComponent(out Enemy enemy);

        return enemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }

    public bool TryGetEnemy(out IDamageable enemy)
    {
        enemy = _currentEnemy;

        if (_currentEnemy == null)
            return false;
        
        return true;
    }

    private IEnumerator FindEnemyJob()
    {
        float delay = 0.25f;
        var wait = new WaitForSeconds(delay);

        while (enabled)
        {
            _currentEnemy = TryFindCollision();

            yield return wait;
        }
    }
}