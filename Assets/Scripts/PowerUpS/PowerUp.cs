using UnityEngine;

public class PowerUp : MonoBehaviour, IPowerUp
{
    public string type;

    public string Type => type;

    public void ApplyEffect(Ball ball)
    {
        ball.ApplyPowerUp(type);
    }

    
}
