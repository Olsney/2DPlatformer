using UnityEngine;

namespace World.Characters.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(int damage);

        Vector3 Position { get; }
    }
}