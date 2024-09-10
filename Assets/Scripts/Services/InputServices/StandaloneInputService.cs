using UnityEngine;

namespace Services.InputServices
{
    public class StandaloneInputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const KeyCode Jump = KeyCode.Space; 
    
        public float DirectionX => Input.GetAxis(Horizontal);
        public bool IsJumping => Input.GetKeyDown(Jump);
    }
}