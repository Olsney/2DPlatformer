using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class PlayerChecker : MonoBehaviour
{
    [SerializeField] private float _checkRadius = 2f;
    [SerializeField] private LayerMask _layerMask;
    
    public event Action<Transform> FoundPlayer;
    public event Action LostPlayer;

    private Coroutine findPlayerJob;
    private Player _player;

    private void Start()
    {
        LostPlayer?.Invoke();
    }

    // private Player TryFindCollision()
    // {
    //     Collider2D collider = Physics2D.OverlapCircle(transform.position, _checkRadius, _layerMask);
    //
    //     if (collider == null)
    //         return null;
    //
    //     collider.TryGetComponent(out Player player);
    //
    //     return player;
    // }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(transform.position, _checkRadius);
    // }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out IEnemyTarget player))
        {
            Debug.Log($"Нашли игрока!");
            FoundPlayer?.Invoke(player.Transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out IEnemyTarget player))
        {
            Debug.Log("Потеряли игрока!");
            LostPlayer?.Invoke();
        }
    }

    // public bool TryGetPlayer(out Player player)
    // {
    //     player = TryFindCollision();
    //
    //     if (player = null)
    //         return false;
    //
    //     return true;
    // }

    // private IEnumerator FindPlayerJob()
    // {
    //     float delay = 0.25f;
    //     var wait = new WaitForSeconds(delay);
    //
    //     while (enabled)
    //     {
    //         TryFindCollision();
    //         
    //         yield return wait;
    //     }
    // }
}