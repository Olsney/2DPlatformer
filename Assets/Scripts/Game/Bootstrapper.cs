using UnityEngine;
using World.Characters.Enemies;
using World.Characters.Players;
using IInitializable = World.Characters.Interfaces.IInitializable;

namespace Game
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Enemy[] _enemies;
        [SerializeField] private IInitializable[] _initializables;
        
        public void Awake()
        {
            InitEnemies();
            
            _player.Init();
        }
        
        private void InitEnemies()
        {
            foreach (Enemy enemy in _enemies)
                enemy.Init();
        }
    }
}