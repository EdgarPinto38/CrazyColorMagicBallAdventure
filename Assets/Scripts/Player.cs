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
        
        if (moverEnX)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * velocidad * Time.deltaTime;
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -limiteX, limiteX), transform.position.y, transform.position.z);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * velocidad * Time.deltaTime;
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -limiteX, limiteX), transform.position.y, transform.position.z);
            }
        }

        
        if (moverEnZ)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.forward * velocidad * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, -limiteZ, limiteZ));
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.back * velocidad * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, -limiteZ, limiteZ));
            }
        }
    }
}
