using UnityEngine;

public static class AnimatorData
{
    public static readonly int Speed = Animator.StringToHash(nameof(Speed));
    public static readonly int Jump = Animator.StringToHash(nameof(Jump));
    public static readonly int IsGrounded = Animator.StringToHash(nameof(IsGrounded));
}