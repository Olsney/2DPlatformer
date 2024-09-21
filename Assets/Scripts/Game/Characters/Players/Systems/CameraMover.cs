using UnityEngine;

namespace World.Characters.Players.Systems
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset = new Vector3(0, 0, -1f);
        [SerializeField] private float _smoothing = 3f;
    
        private void LateUpdate()
        {
            Move();
        }

        private void Move()
        {
            var nextPosition = Vector3.Lerp(transform.position, _target.position + _offset, _smoothing * Time.fixedTime);

            transform.position = nextPosition;
        }
    }
}