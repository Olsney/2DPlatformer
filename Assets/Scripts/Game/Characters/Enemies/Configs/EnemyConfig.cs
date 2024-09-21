using UnityEngine;

namespace World.Characters.Enemies.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private int _damage = 5;
    
        public int MaxHealth => _maxHealth;
        public int Damage => _damage;
    }
}