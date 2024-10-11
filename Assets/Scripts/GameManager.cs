using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject ballObject;
    public Ball ball; // Referencia al script de la pelota

    public Cube[] cubes; // Arreglo de cubos

    public GameObject[] walls;
    public GameObject PanelGame;
    public GameObject PanelWin;
    public GameObject PanelGameOver;
    public TextMeshProUGUI countdownText;

    void Start()
    {
        StartLevelCountdown(3);
        Time.timeScale = 1;
    }

    void Update()
    {
        CheckBallPosition();
        CheckCubesLives(); // Verificar constantemente si los cubos 0, 1 y 2 tienen 0 vidas
    }

    void CheckBallPosition()
    {
        // Verifica si la pelota ha pasado los límites en el eje X o Z
        if (ball.transform.position.x > ball.limit)
        {
            if (cubes[0] != null)
            {
                cubes[0].LoseLife();
                if (cubes[0].lives <= 0)
                {
                    cubes[0] = null; 
                    walls[0].SetActive(true);
                }
            }
        }
        else if (ball.transform.position.x < -ball.limit)
        {
            if (cubes[1] != null)
            {
                cubes[1].LoseLife();
                if (cubes[1].lives <= 0)
                {
                    cubes[1] = null;
                    walls[1].SetActive(true);
                }
            }
        }
        else if (ball.transform.position.z > ball.limit)
        {
            if (cubes[2] != null)
            {
                cubes[2].LoseLife();
                if (cubes[2].lives <= 0)
                {
                    cubes[2] = null;
                    walls[2].SetActive(true);
                }
            }
        }
        else if (ball.transform.position.z < -ball.limit)
        {
            if (cubes[3] != null)
            {
                cubes[3].LoseLife();
                if (cubes[3].lives <= 0)
                {
                    cubes[3] = null;
                    walls[3].SetActive(true);
                    PanelGame.SetActive(false);
                    PanelGameOver.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
    }

    // Método para verificar si los cubos 0, 1 y 2 tienen 0 vidas
    void CheckCubesLives()
    {
        bool allCubesDead = true;

        for (int i = 0; i < 3; i++)
        {
            if (cubes[i] != null && cubes[i].lives > 0)
            {
                allCubesDead = false;
                break; 
            }
        }

        if (allCubesDead)
        {
            Win();
        }
    }

    // Método para ganar el juego
    private void Win()
    {
        PanelGame.SetActive(false); 
        PanelWin.SetActive(true);
        Time.timeScale = 0; 
    }

    public void StartLevelCountdown(int countdownTime)
    {
        StartCoroutine(CountdownCoroutine(countdownTime));
    }

    private IEnumerator CountdownCoroutine(int countdownTime)
    {
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString(); 
            yield return new WaitForSeconds(1);
            countdownTime--;
        }
        countdownText.text = "¡GO!"; 
        yield return new WaitForSeconds(1);
        countdownText.text = ""; 
        ballObject.SetActive(true);
    }
}
