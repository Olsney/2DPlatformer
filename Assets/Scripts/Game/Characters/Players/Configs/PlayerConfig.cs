using UnityEngine;

namespace World.Characters.Players.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private int _damage = 20;
        [SerializeField] private int _maxHealth = 200;

        public int Damage => _damage;
        public int MaxHealth => _maxHealth;
    }
}