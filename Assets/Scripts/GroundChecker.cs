using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float _checkRadius = 0.2f;
    [SerializeField] private LayerMask _layerMask;
    
    public bool IsGrounded { get; private set; }

    private void Update()
    {
        IsGrounded = IsCollisionsExist();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }

    private bool IsCollisionsExist() =>
        Physics2D.OverlapCircle(transform.position, _checkRadius, _layerMask);
}