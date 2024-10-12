using Game.UI;
using UnityEngine;
using World.Characters.Enemies;
using World.Characters.Players;

namespace Game
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Enemy[] _enemies;
        [SerializeField] private HealthPresenter _healthPresenter;
        
        public void Awake()
        {
            InitEnemies();
            _player.Init();
            _healthPresenter.Init();
        }
        
        private void InitEnemies()
        {
            foreach (Enemy enemy in _enemies)
                enemy.Init();
        }
    }
}