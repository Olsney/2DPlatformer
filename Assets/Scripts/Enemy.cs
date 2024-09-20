using DefaultNamespace;
using UnityEngine;

public class Enemy : MonoBehaviour, IAttacker, IDamageable
{
    [SerializeField] private PlayerChecker _playerChecker;

    private int _health;
    private int _damage;
    private EnemyMover _mover;

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
    }

    private void OnEnable()
    {
        _playerChecker.FoundPlayer += _mover.MoveToPlayer;
        _playerChecker.LostPlayer += _mover.MoveToPoint;
    }

    private void OnDisable()
    {
        _playerChecker.FoundPlayer -= _mover.MoveToPlayer;
        _playerChecker.LostPlayer -= _mover.MoveToPoint;
    }

    private void Update()
    {
        // _mover.MoveToPoint();
        //
        // if (_mover.IsOnPoint())
        // {
        //     _mover.ChangePoint();
        //     _mover.TryFlip();
        // }
        
        
        
        // if (_playerChecker.TryGetPlayer(out Player player))
        // {
        //     Debug.Log($"Вижу игрока!");
        //
        //     _mover.MoveToPlayer(player);
        //
        //     Debug.Log("Пошел к игроку");
        //
        //     return;
        // }
    }

    public void Attack(IDamageable player)
    {
        player.TakeDamage(_damage);
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            damage = 0;

        _health -= damage;

        if (_health < 0)
        {
            Destroy(gameObject);
        }
    }
}