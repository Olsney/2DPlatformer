using System;
using UnityEngine;
using World.Characters.Interfaces;
using World.Characters.Players;

namespace World.Characters.Enemies.Systems
{
    public class AttackAbility : MonoBehaviour
    {
        public event Action<IDamageable> Founded;
        public event Action Lost;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out IDamageable player))
            {
                Founded?.Invoke(player);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out IDamageable _))
            {
                Lost?.Invoke();
            }
        }
    }
}