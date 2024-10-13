using System;
using UnityEngine;
using World.Characters.Interfaces;
using World.Characters.Players;

namespace World.Characters.Enemies.Systems
{
    public class PlayerFinder : FinderBase<IEnemyTarget>
    {
        // public event Action<IEnemyTarget> Found;
        // public event Action Lost;
        //
        // private void Start()
        // {
        //     Lost?.Invoke();
        // }
        //
        // private void OnTriggerEnter2D(Collider2D collider)
        // {
        //     if (collider.TryGetComponent(out IEnemyTarget player))
        //     {
        //         Found?.Invoke(player);
        //     }
        // }
        //
        // private void OnTriggerExit2D(Collider2D collider)
        // {
        //     if (collider.TryGetComponent(out IEnemyTarget player))
        //     {
        //         Lost?.Invoke();
        //     }
        // }
    }
}