using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
   public float speed = 5f;
    public Vector3 direction;
    public float xLimit = 10f;
    public float zLimit = 5f;

    void Start()
    {
        direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        
        if (transform.position.x > xLimit || transform.position.x < -xLimit)
        {
            direction.x *= -1;
        }

        if (transform.position.z > zLimit || transform.position.z < -zLimit)
        {
            direction.z *= -1;
        }
    }

}
