using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Coin))]
public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Coin _coinPrefab;

    private void Awake()
    {
        _spawnPoints = GetComponentsInChildren<Transform>();

        Spawn();
    }

    private void Spawn()
    {
        for(int i = 0; i < _spawnPoints.Length; i++)
            Instantiate(_coinPrefab, _spawnPoints[i].position, Quaternion.identity);
    }
}