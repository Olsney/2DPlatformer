using Animators;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class Player : MonoBehaviour, IAttacker, IDamageable, IHealeable, IEnemyTarget
{
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private EnemyChecker _enemyChecker;

    private PlayerMover _mover;
    private int _damage;
    private int _health;

    public Transform Transform => transform;

    //Как прописать логику атаки врага, что если у меня энемичекер нашел врага, то начать к нему двигаться, а если уже расстояние достаточное для атаки, то наносить атаки?

    private void Awake()
    {
        //Комментарий от Вениамина:
        //В Player есть Awake и в PlayerMover тоже есть, зачем? У тебя Player знает о PlayerMover инициализируй через плеера его мувмент.
        //Как понять этот комментарий? Что нужно сделать?
        _mover = GetComponent<PlayerMover>();
        _health = 100;
    }

    private void Update()
    {
        _mover.CalculateDirection();

        if (_enemyChecker.TryGetEnemy(out IDamageable enemy) == false)
            return;

        Attack(enemy);
    }

    private void FixedUpdate()
    {
        _mover.Move();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Coin coin))
            Destroy(coin.gameObject);

        if (collider.TryGetComponent(out HealingKit healingKit))
        {
            healingKit.Heal(this);
            Destroy(healingKit.gameObject);
            //Это будет работать? 
            //Правильно ли я прописал логику хила?
        }
    }

    public void Attack(IDamageable enemy)
    {
        //У энеми такой же метод атаки и получения урона. Можно ли вынести в класс Attacker и Damagable с базовой реализацией,
        //которые будут наследовать интерфейсы и просто прокинуть их сюда?
        //Или это норм так как уже есть интерфейсы? Тогда просто тут и у врага получится
        //_attacker.Attack(enemy) и _damageable.TakeDamage(damage)

        // if (_enemyChecker.TryGetEnemy(out IDamageable enemy) == false)
        //     return;
        
        enemy.TakeDamage(_damage);
        _animator.Attack();
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            damage = 0;

        if (_health < 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeHeal(int heal)
    {
        if (heal < 0)
            heal = 0;

        _health += heal;
        Debug.Log($"Пополнили здоровье. Текущее здоровье: {_health}");
    }
}