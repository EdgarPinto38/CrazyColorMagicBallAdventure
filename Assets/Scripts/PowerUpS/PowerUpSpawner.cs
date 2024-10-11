using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; 
    public float spawnRangeX = 3.5f;
    public float spawnRangeZ = 3.5f;
    public float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating("SpawnPowerUp", spawnInterval, spawnInterval);
    }

    void SpawnPowerUp()
    {
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        float z = Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnPosition = new Vector3(x, 2f, z); // Asegúrate de que el eje Y no cambie

        // Elegir un power-up aleatorio del array
        int randomIndex = Random.Range(0, powerUpPrefabs.Length);
        GameObject powerUpPrefab = powerUpPrefabs[randomIndex];

        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }
}
