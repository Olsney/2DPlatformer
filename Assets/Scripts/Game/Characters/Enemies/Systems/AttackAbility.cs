using System;
using UnityEngine;
using World.Characters.Players;

namespace World.Characters.Enemies.Systems
{
    public class AttackAbility : MonoBehaviour
    {
        public event Action<Player> Founded;
        public event Action Lost;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Player player))
            {
                Founded?.Invoke(player);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Player _))
            {
                Lost?.Invoke();
            }
        }
    }
}