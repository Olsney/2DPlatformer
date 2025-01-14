using UnityEngine;

namespace Services.InputServices
{
    public class StandaloneInputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const KeyCode Jump = KeyCode.Space;
        private const KeyCode UseDrainHealth = KeyCode.G;
    
        public float DirectionX => Input.GetAxis(Horizontal);
        public bool IsJumping => Input.GetKeyDown(Jump);
        public bool IsDrainHealthUsed => Input.GetKeyDown(UseDrainHealth);
    }
}