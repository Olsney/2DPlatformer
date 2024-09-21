using Unity.VisualScripting;
using UnityEngine;
using World.Characters.Enemies;
using World.Characters.Players;

namespace Game
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Enemy[] _enemies;
        
        public void Awake()
        {
            _player.Init();
    
            InitEnemies();
        }
    
        private void InitEnemies()
        {
            foreach (Enemy enemy in _enemies)
                enemy.Init();
        }
    }
}