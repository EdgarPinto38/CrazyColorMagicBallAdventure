using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocidad = 5f; 
    public bool moverEnX = true;  
    public bool moverEnZ = true;  

    private float limiteX = 3f; 
    private float limiteZ = 3f; 

    void Update()
    {
        // Obtener el input del teclado y el joystick
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 

        // Movimiento en el eje X
        if (moverEnX)
        {
            transform.position += Vector3.right * horizontal * velocidad * Time.deltaTime;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -limiteX, limiteX), transform.position.y, transform.position.z);
        }

        // Movimiento en el eje Z
        if (moverEnZ)
        {
            transform.position += Vector3.forward * vertical * velocidad * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, -limiteZ, limiteZ));
        }
    }
}
