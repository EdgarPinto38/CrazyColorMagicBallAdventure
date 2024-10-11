using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public enum ErrorType { RandomMove, StayStill };
    public ErrorType[] possibleErrors = new ErrorType[] { ErrorType.RandomMove, ErrorType.StayStill };

    public List<Transform> balls; // Lista de bolas que la IA seguirá
    public float moveSpeed = 5f;
    public bool moveInX = true; // Indica si la IA se moverá en el eje X o Z
    public float errorChance = 0.5f; // Probabilidad de error
    public float randomMoveRange = 5f; // Rango de movimiento aleatorio
    private float limit = 3f;

    private Vector3 targetPosition;
    private bool isStationary = false;
    private bool isInErrorState = false; // Bandera para saber si está en un estado de error

    private float errorDuration = 0f; // Duración del error
    private float stationaryTime = 0f;
    private float fixedPosition; // Eje fijo
    private Vector3 randomTargetPosition; // Objetivo para el movimiento aleatorio
    private float originalMoveSpeed;

    void Start()
    {
        // Fijar la posición en el eje que no se moverá (X o Z)
        fixedPosition = moveInX ? transform.position.z : transform.position.x;

        targetPosition = transform.position;
        originalMoveSpeed = moveSpeed;
        StartCoroutine(CheckForErrors());
    }

    IEnumerator CheckForErrors()
    {
        while (true)
        {
            if (!isInErrorState && !isStationary && Random.value < errorChance)
            {
                TriggerErrorState();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void TriggerErrorState()
    {
        if (possibleErrors == null || possibleErrors.Length == 0)
        {
            Debug.LogError("El arreglo possibleErrors está vacío o no está inicializado");
            return;
        }

        ErrorType error = possibleErrors[Random.Range(0, possibleErrors.Length)];
        switch (error)
        {
            case ErrorType.RandomMove:
                randomTargetPosition = GetRandomTargetPosition();
                errorDuration = Random.Range(1f, 2f);
                isInErrorState = true;
                StartCoroutine(HandleErrorState());
                break;

            case ErrorType.StayStill:
                isStationary = true;
                stationaryTime = Random.Range(0.5f, 1f);
                StartCoroutine(HandleStationaryState());
                break;
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
        yield return new WaitForSeconds(stationaryTime);
        isStationary = false;
        moveSpeed = originalMoveSpeed;
    }

    void Update()
    {
        if (!isInErrorState && !isStationary)
        {
            MoveTowardsClosestBall();
            ClampPositionWithinLimits();
        }
    }

    void MoveTowardsClosestBall()
    {
        if (balls.Count > 0)
        {
            Transform closestBall = GetClosestBall();
            targetPosition = closestBall.position;
            MoveTowardsTarget(targetPosition);
        }
    }

    Transform GetClosestBall()
    {
        Transform closestBall = null;
        float closestDistance = Mathf.Infinity;
        foreach (Transform ball in balls)
        {
            float distance = Vector3.Distance(transform.position, ball.position);
            if (distance < closestDistance)
            {
                closestBall = ball;
                closestDistance = distance;
            }
        }
        return closestBall;
    }

    void MoveTowardsTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0; // Ignorar la altura
        direction.Normalize();
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    Vector3 GetRandomTargetPosition()
    {
        float randomX = Random.Range(-randomMoveRange, randomMoveRange);
        float randomZ = Random.Range(-randomMoveRange, randomMoveRange);
        return moveInX ? new Vector3(Mathf.Clamp(transform.position.x + randomX, -limit, limit), transform.position.y, fixedPosition) : new Vector3(fixedPosition, transform.position.y, Mathf.Clamp(transform.position.z + randomZ, -limit, limit));
    }

    void ExecuteErrorBehavior()
    {
        if (moveInX)
        {
            transform.position = new Vector3(
                Mathf.MoveTowards(transform.position.x, randomTargetPosition.x, moveSpeed * Time.deltaTime),
                transform.position.y,
                fixedPosition
            );
        }
        else
        {
            transform.position = new Vector3(
                fixedPosition,
                transform.position.y,
                Mathf.MoveTowards(transform.position.z, randomTargetPosition.z, moveSpeed * Time.deltaTime)
            );
        }
    }

    void ClampPositionWithinLimits()
    {
        float clampedX = Mathf.Clamp(transform.position.x, -limit, limit);
        float clampedZ = Mathf.Clamp(transform.position.z, -limit, limit);
        transform.position = moveInX ? new Vector3(clampedX, transform.position.y, fixedPosition) : new Vector3(fixedPosition, transform.position.y, clampedZ);
    }
}