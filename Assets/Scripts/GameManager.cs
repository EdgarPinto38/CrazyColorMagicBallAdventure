using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject ballObject;
    public Ball ball;

    public Cube[] cubes; // Arreglo de cubos

    public GameObject[] walls;
    public List<GameObject> balls = new List<GameObject>();
    public GameObject PanelGame;
    public GameObject PanelGameOver;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI countdownText;


    private AI ai;
    public GameObject ballPrefab;


    void Start()
    {
        StartLevelCountdown(3);
        Time.timeScale = 1;
        StartCoroutine(SpawnAdditionalBalls());
        ai = FindObjectOfType<AI>();
    }

    void Update()
    {
        CheckBallPosition();
        CheckCubesLives(); // Verificar constantemente si los cubos (excepto el cubo del jugador) tienen 0 vidas
    }
    private IEnumerator SpawnAdditionalBalls()
    {
        List<float> waitTimes = new List<float> { 3.5f, 10f, 20f, 30f, 40f };

        for (int i = 0; i < waitTimes.Count; i++)
        {
            yield return new WaitForSeconds(waitTimes[i]);
            if (i < balls.Count)
            {
                balls[i].SetActive(true);
            }
            else
            {
                Debug.LogWarning("No hay suficientes bolas en la lista para activar.");
            }
        }
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

    // Método para verificar si los cubos (excepto el cubo del jugador) tienen 0 vidas
    void CheckCubesLives()
    {
        bool allCubesDead = true;

        for (int i = 0; i < cubes.Length; i++)
        {
            if (i == 3) // Excluir el cubo 4 que representa al jugador
                continue;

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
