using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cube : MonoBehaviour
{
    public int lives = 10; // Número inicial de vidas
    public TextMeshProUGUI livesText; 

    void Start()
    {
        UpdateLivesUI(); 
    }

    public void LoseLife()
    {
        lives--;
        Debug.Log($"{name} perdió una vida. Vidas restantes: {lives}");

        UpdateLivesUI(); 

        if (lives <= 0)
        {
            Destroy(gameObject); 
        }
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
        {
           
            livesText.text = $"X{lives}"; 
        }
    }
}
