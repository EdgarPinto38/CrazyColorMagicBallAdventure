using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5f;
    private float speedIncreaseFactor = 1.1f;

    private Rigidbody rb;

    private Vector3 direction;

    private float limit = 5.5f;

    // Rango de variación aleatoria al reflejar la dirección (para hacer el rebote menos predecible)
    public float randomBounceFactor = 0.3f; // Puedes ajustar este valor según prefieras

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Inicializar una dirección aleatoria en el plano XZ
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        rb.velocity = direction * speed;
    }

    void Update()
    {
        Reset();
    }

    private void Reset()
{
    if ((transform.position.z > limit || transform.position.z < -limit) || (transform.position.x > limit || transform.position.x < -limit))
    {
        speed = 5;
        transform.position = new Vector3(0, 2, 0);
        
        // Add random factor to direction
        direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        
        // Ensure it's not too predictable by adding a small random factor
        direction.x += Random.Range(-randomBounceFactor, randomBounceFactor);
        direction.z += Random.Range(-randomBounceFactor, randomBounceFactor);

        direction = direction.normalized;

        rb.velocity = direction * speed;
    }
}


    void OnCollisionEnter(Collision collision)
    {
        // Invertir la componente de la velocidad perpendicular a la superficie de colisión
        Vector3 normal = collision.contacts[0].normal;
        direction = Vector3.Reflect(direction, normal);

        // Agregar variación aleatoria en los ejes X y Z tras el rebote
        float randomX = Random.Range(-randomBounceFactor, randomBounceFactor);
        float randomZ = Random.Range(-randomBounceFactor, randomBounceFactor);

        // Aplicar las variaciones aleatorias a la dirección
        direction.x += randomX;
        direction.z += randomZ;

        // Normalizar la dirección para mantener la velocidad uniforme
        direction = direction.normalized;

        // Incrementar la velocidad en cada colisión
        speed *= speedIncreaseFactor;
        rb.velocity = direction * speed;
    }
}
