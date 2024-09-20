using UnityEngine;

public static class AnimatorData
{
    public static class PlayerData
    {
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
        public static readonly int IsGrounded = Animator.StringToHash(nameof(IsGrounded));
        public static readonly int Attack = Animator.StringToHash(nameof(Attack));
    }
}