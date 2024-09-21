using System.Collections;
using UnityEngine;

namespace World.Characters.Players.Systems
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private float _checkRadius = 0.5f;
        [SerializeField] private LayerMask _layerMask;

        private Coroutine _checkGroundJob;

        public bool IsGrounded { get; private set; }

        private void Start()
        {
            _checkGroundJob = StartCoroutine(CheckGroundJob());
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _checkRadius);
        }

        private bool IsCollisionsExist() =>
            Physics2D.OverlapCircle(transform.position, _checkRadius, _layerMask);

        private IEnumerator CheckGroundJob()
        {
            float delay = 0.1f;
            var wait = new WaitForSeconds(delay);

            while (enabled)
            {
                IsGrounded = IsCollisionsExist();

                yield return wait;
            }
        }
    }
}