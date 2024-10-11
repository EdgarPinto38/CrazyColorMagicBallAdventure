using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject ballObject; // Prefab de la pelota
    public Ball ball; // Referencia al script de la pelota principal

    public Cube[] cubes; // Arreglo de cubos

    public GameObject[] walls;
    public GameObject PanelGame;
    public GameObject PanelGameOver;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI countdownText;

    private List<GameObject> balls = new List<GameObject>(); // Lista para las bolas activas

    void Start()
    {
        StartLevelCountdown(3);
        Time.timeScale = 1;
        balls.Add(ballObject); // Agregar la primera bola a la lista
        StartCoroutine(SpawnAdditionalBalls()); // Iniciar la corutina para crear más bolas
    }

    void Update()
    {
        CheckBallPosition();
        CheckCubesLives(); // Verificar constantemente si los cubos 0, 1 y 2 tienen 0 vidas
    }

    // Método para generar bolas adicionales después de un tiempo determinado
    private IEnumerator SpawnAdditionalBalls()
    {
        yield return new WaitForSeconds(10); // Espera de 10 segundos para la segunda bola
        SpawnBall();
        
        yield return new WaitForSeconds(15); // Espera hasta completar 25 segundos para la tercera bola
        SpawnBall();
        
        yield return new WaitForSeconds(30); // Espera hasta completar 55 segundos para la cuarta bola
        SpawnBall();
        
        yield return new WaitForSeconds(35); // Espera hasta completar 90 segundos para la quinta bola
        SpawnBall();
    }

    // Método para instanciar una nueva bola
    private void SpawnBall()
    {
        GameObject newBall = Instantiate(ballObject, ballObject.transform.position, Quaternion.identity); // Crear una nueva bola en la misma posición que la original
        balls.Add(newBall); // Añadir la nueva bola a la lista de bolas activas
        newBall.SetActive(true); // Activar la nueva bola
    }

    void CheckBallPosition()
    {
        foreach (GameObject activeBall in balls)
        {
            Ball ballScript = activeBall.GetComponent<Ball>();
            if (ballScript.transform.position.x > ballScript.limit)
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
            else if (ballScript.transform.position.x < -ballScript.limit)
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
            else if (ballScript.transform.position.z > ballScript.limit)
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
            else if (ballScript.transform.position.z < -ballScript.limit)
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
                        gameOverText.text = "You Lose";
                        Time.timeScale = 0;
                    }
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
        PanelGameOver.SetActive(true);
        gameOverText.text = "You win!";
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
