using System;
using UnityEngine;

namespace World.Characters.Enemies.Systems
{
    public class FinderBase<T> : MonoBehaviour 
    {
        public event Action<T> Found;
        public event Action Lost;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out T component))
            {
                Found?.Invoke(component);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out T _))
            {
                Lost?.Invoke();
            }
        }
    }
}