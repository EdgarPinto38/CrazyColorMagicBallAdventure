public interface IPowerUp
{
    string Type { get; }
    void ApplyEffect(Ball ball);
}
