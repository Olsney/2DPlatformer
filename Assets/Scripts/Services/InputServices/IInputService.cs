namespace Services.InputServices
{
    public interface IInputService
    {
        float DirectionX { get; }
        bool IsJumping { get; }
    }
}