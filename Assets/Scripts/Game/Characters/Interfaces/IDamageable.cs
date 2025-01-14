using UnityEngine;

namespace World.Characters.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(int damage);

        Vector3 Position { get; }
        bool IsDestroyed { get; }
        
        int Health { get; }
    }
}