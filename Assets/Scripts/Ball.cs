using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5f;
    public float size = 1f;
    private float speedIncreaseFactor = 1.1f;

    private Rigidbody rb;

    private Vector3 direction;

    public float limit = 5.5f;
    public float randomBounceFactor = 0.3f;
    public float minSpeedThreshold = 0.1f; // Umbral mínimo de velocidad para considerar la bola quieta
    public float timeToTrigger = 0.5f; // Tiempo en segundos para que la bola esté quieta antes de ejecutar la acción

    private float idleTime = 0f; // Tiempo que la bola ha estado quieta

    // Posiciones predefinidas para el inicio de la bola
    public Vector3[] startPositions = new Vector3[4]; // Arreglo de 4 posiciones predefinidas
    private Coroutine sizeCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetRandomStartPositionAndDirection(); // Colocar la bola en una posición inicial aleatoria con dirección hacia el centro en los ejes X y Z
    }

    void Update()
    {
        Reset();
        CheckIfBallIsIdle();
    }

    private void Reset()
    {
        if ((transform.position.z > limit || transform.position.z < -limit) || (transform.position.x > limit || transform.position.x < -limit))
        {
            speed = 5;
            SetRandomStartPositionAndDirection(); // Colocar la bola en una posición aleatoria con dirección hacia el centro en los ejes X y Z
        }
    }

    private void CheckIfBallIsIdle()
    {
        // Verificar si la velocidad de la bola es menor al umbral mínimo
        if (rb.velocity.magnitude < minSpeedThreshold)
        {
            idleTime += Time.deltaTime; // Incrementar el contador de tiempo inactivo
            if (idleTime >= timeToTrigger)
            {
                OnBallIdle(); // Ejecutar la acción cuando la bola esté quieta por más de 1 segundo
            }
        }
        else
        {
            idleTime = 0f;
        }
    }

    private void OnBallIdle()
    {
        Debug.Log("La bola ha estado quieta por 1 segundo.");
        speed = 5;
        SetRandomStartPositionAndDirection(); // Colocar la bola en una posición aleatoria con dirección hacia el centro en los ejes X y Z
    }

    // Método para seleccionar una posición inicial aleatoria y su dirección hacia el centro en los ejes X y Z
    private void SetRandomStartPositionAndDirection()
    {
        int randomIndex = Random.Range(0, startPositions.Length); // Seleccionar una posición aleatoria del arreglo
        transform.position = startPositions[randomIndex]; // Asignar la posición aleatoria a la bola

        // Calcular la dirección hacia el centro del campo (0, 0) solo en los ejes X y Z
        Vector3 directionToCenter = new Vector3(-transform.position.x, 0, -transform.position.z).normalized; // Mantener Y igual a 0

        // Aplicar la velocidad en esa dirección (solo afectando X y Z)
        rb.velocity = directionToCenter * speed;
    }

    public void ApplyPowerUp(string powerUp)
    {
        switch (powerUp)
        {
            case "slow_down":
                speed = Mathf.Max(speed - 1, 0); // No menos de 0
                break;
            case "change_size":
                size += 0.5f;
                transform.localScale = new Vector3(size, size, size);
                if (sizeCoroutine != null)
                {
                    StopCoroutine(sizeCoroutine);
                }
                sizeCoroutine = StartCoroutine(RevertSizeAfterTime(2f));
                break;
            case "reverse_direction":
                direction *= -1;
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        direction = Vector3.Reflect(direction, normal);

        // Agregar variación aleatoria en los ejes X y Z tras el rebote
        float randomX = Random.Range(-randomBounceFactor, randomBounceFactor);
        float randomZ = Random.Range(-randomBounceFactor, randomBounceFactor);

        direction.x += randomX;
        direction.z += randomZ;

        direction = direction.normalized;

        // Incrementar la velocidad en cada colisión
        speed *= speedIncreaseFactor;
        rb.velocity = direction * speed;

        // Detectar si el objeto colisionado es un power-up
        IPowerUp powerUp = collision.gameObject.GetComponent<IPowerUp>();
        if (powerUp != null)
        {
            powerUp.ApplyEffect(this);
            Destroy(collision.gameObject);
        }


    }

    private IEnumerator RevertSizeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        size -= 0.5f;
        transform.localScale = new Vector3(size, size, size);
    }
}
