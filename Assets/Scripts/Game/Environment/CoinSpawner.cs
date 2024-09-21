using UnityEngine;

namespace World.Environment
{
    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Coin _coinPrefab;

        private void Awake()
        {
            Spawn();
        }

        private void Spawn()
        {
            for(int i = 0; i < _spawnPoints.Length; i++)
                Instantiate(_coinPrefab, _spawnPoints[i].position, Quaternion.identity);
        }
    }
}