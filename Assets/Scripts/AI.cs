using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public enum ErrorType { RandomMove, StayStill };
    public ErrorType[] possibleErrors = new ErrorType[] { ErrorType.RandomMove, ErrorType.StayStill };

    public Transform ball; // La pelota que la IA seguirá
    public float moveSpeed = 5f;
    public bool moveInX = true; // Indica si la IA se moverá en el eje X o Z
    public float errorChance = 0.5f; // Probabilidad de error
    public float randomMoveRange = 5f; // Rango de movimiento aleatorio
    private float limit = 3f;

    private Vector3 targetPosition;
    private bool isStationary = false;
    private float stationaryTime = 0f;

    private bool isInErrorState = false; // Bandera para saber si está en un estado de error
    private float errorDuration = 0f; // Duración del error
    private Vector3 randomTargetPosition; // Objetivo para cuando ocurra un movimiento aleatorio

    private float fixedPosition; // Para fijar el eje que no se debe mover
    private float originalMoveSpeed;

    void Start()
    {
        // Fijar la posición en el eje que no se moverá (X o Z)
        if (moveInX)
        {
            fixedPosition = transform.position.z; // Si se mueve en X, fijar Z
        }
        else
        {
            fixedPosition = transform.position.x; // Si se mueve en Z, fijar X
        }

        targetPosition = transform.position;
        originalMoveSpeed = moveSpeed;
        isInErrorState = false;
        isStationary = false;
        StartCoroutine(CheckForErrors());

    }

    IEnumerator CheckForErrors()
    {
        while (true)
        {
            if (!isInErrorState && !isStationary && Random.value < errorChance)
            {
                
                if (possibleErrors != null && possibleErrors.Length > 0)
                {
                    int randomIndex = Random.Range(0, possibleErrors.Length);
                    ErrorType error = possibleErrors[randomIndex];
                    switch (error)
                    {
                        case ErrorType.RandomMove:
                            float randomX = Random.Range(-randomMoveRange, randomMoveRange);
                            float randomZ = Random.Range(-randomMoveRange, randomMoveRange);
                            if (moveInX)
                                randomTargetPosition = new Vector3(Mathf.Clamp(transform.position.x + randomX, -limit, limit), transform.position.y, fixedPosition);
                            else
                                randomTargetPosition = new Vector3(fixedPosition, transform.position.y, Mathf.Clamp(transform.position.z + randomZ, -limit, limit));

                            errorDuration = Random.Range(1f, 3f);
                            isInErrorState = true;
                            StartCoroutine(HandleErrorState());
                            break;

                        case ErrorType.StayStill:
                            moveSpeed = 0;
                            isStationary = true;
                            stationaryTime = Random.Range(1f, 3f);
                            StartCoroutine(HandleStationaryState());
                            break;
                    }
                }
                else
                {
                   Debug.LogError("El arreglo possibleErrors está vacío o no está inicializado.");
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator HandleErrorState()
    {
        while (errorDuration > 0)
        {
            errorDuration -= Time.deltaTime;
            ExecuteErrorBehavior();
            yield return null;
        }
        isInErrorState = false;
    }

    IEnumerator HandleStationaryState()
    {
        while (stationaryTime > 0)
        {
            stationaryTime -= Time.deltaTime;
            yield return null;
        }
        isStationary = false;
        moveSpeed = originalMoveSpeed;
    }

    void Update()
    {
        if (!isInErrorState && !isStationary)
        {
            Vector3 ballPosition = ball.position;
            Vector3 newTargetPosition = ballPosition;

            targetPosition = newTargetPosition;

            if (moveInX)
            {
                transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, targetPosition.x, moveSpeed * Time.deltaTime), transform.position.y, fixedPosition);
            }
            else
            {
                transform.position = new Vector3(fixedPosition, transform.position.y, Mathf.MoveTowards(transform.position.z, targetPosition.z, moveSpeed * Time.deltaTime));
            }

            ClampPositionWithinLimits();
        }
    }



    // Método para asegurarse de que el cubo se mantenga dentro de los límites
    void ClampPositionWithinLimits()
    {
        float clampedX = Mathf.Clamp(transform.position.x, -limit, limit);
        float clampedZ = Mathf.Clamp(transform.position.z, -limit, limit);

        if (moveInX)
        {
            transform.position = new Vector3(clampedX, transform.position.y, fixedPosition); // Fijar Z
        }
        else
        {
            transform.position = new Vector3(fixedPosition, transform.position.y, clampedZ); // Fijar X
        }
    }

    // Reanuda el movimiento después de quedarse quieto
    void ResumeMovement()
    {
        moveSpeed = 5f; 
        isStationary = false;
    }

    // Ejecuta el comportamiento de error mientras dura
    void ExecuteErrorBehavior()
    {
        if (moveInX)
        {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, randomTargetPosition.x, moveSpeed * Time.deltaTime), transform.position.y, fixedPosition); // Movimiento aleatorio en X
        }
        else
        {
            transform.position = new Vector3(fixedPosition, transform.position.y, Mathf.MoveTowards(transform.position.z, randomTargetPosition.z, moveSpeed * Time.deltaTime)); // Movimiento aleatorio en Z
        }
    }
}
