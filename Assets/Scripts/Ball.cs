using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Constantes
    private const float DEFAULT_SPEED = 5f;
    private const float SPEED_INCREASE_FACTOR = 1.1f;
    private const float MIN_SPEED_THRESHOLD = 0.1f;
    private const float TIME_TO_TRIGGER_IDLE = 0.5f;
    private const float SIZE_INCREMENT = 0.5f;
    private const float SIZE_DURATION = 2f;

    public float speed = DEFAULT_SPEED;
    public float size = 1f;
    public float limit = 5.5f;
    public float randomBounceFactor = 0.3f;
    public Vector3[] startPositions = new Vector3[4]; // Posiciones predefinidas

    private Rigidbody rb;
    private Vector3 direction;
    private float idleTime = 0f;
    private Coroutine sizeCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetRandomStartPositionAndDirection();
    }

    void Update()
    {
        ResetIfOutOfBounds();
        CheckIfBallIsIdle();
    }

    private void SetRandomStartPositionAndDirection()
    {
        int randomIndex = Random.Range(0, startPositions.Length);
        transform.position = startPositions[randomIndex];
        SetDirectionToCenter();
        ApplyVelocity();
    }

    private void SetDirectionToCenter()
    {
        direction = new Vector3(-transform.position.x, 0, -transform.position.z).normalized;
    }

    private void ApplyVelocity()
    {
        rb.velocity = direction * speed;
    }

    private void ResetIfOutOfBounds()
    {
        if (IsOutOfBounds())
        {
            speed = DEFAULT_SPEED;
            SetRandomStartPositionAndDirection();
        }
    }

    private bool IsOutOfBounds()
    {
        return transform.position.z > limit || transform.position.z < -limit || transform.position.x > limit || transform.position.x < -limit;
    }

    private void CheckIfBallIsIdle()
    {
        if (rb.velocity.magnitude < MIN_SPEED_THRESHOLD)
        {
            idleTime += Time.deltaTime;
            if (idleTime >= TIME_TO_TRIGGER_IDLE)
            {
                OnBallIdle();
            }
        }
        else
        {
            idleTime = 0f;
        }
    }

    private void OnBallIdle()
    {
        speed = DEFAULT_SPEED;
        SetRandomStartPositionAndDirection();
    }

    public void ApplyPowerUp(string powerUp)
    {
        switch (powerUp)
        {
            case "slow_down":
                SlowDown();
                break;
            case "change_size":
                ChangeSize();
                break;
            case "reverse_direction":
                ReverseDirection();
                break;
        }
    }

    private void SlowDown()
    {
        speed = Mathf.Max(speed - 2, 0);
        ApplyVelocity();
    }

    private void ChangeSize()
    {
        size += SIZE_INCREMENT;
        transform.localScale = new Vector3(size, size, size);

        if (sizeCoroutine != null)
        {
            StopCoroutine(sizeCoroutine);
        }

        sizeCoroutine = StartCoroutine(RevertSizeAfterTime(SIZE_DURATION));
    }

    private void ReverseDirection()
    {
        direction *= -1;
        ApplyVelocity();
    }

    void OnCollisionEnter(Collision collision)
    {
        ReflectDirection(collision.contacts[0].normal);
        AddRandomBounce();
        ApplyVelocity();
        IncrementSpeed();

        HandlePowerUpCollision(collision.gameObject);
    }

    private void ReflectDirection(Vector3 normal)
    {
        direction = Vector3.Reflect(direction, normal);
    }

    private void AddRandomBounce()
    {
        direction.x += Random.Range(-randomBounceFactor, randomBounceFactor);
        direction.z += Random.Range(-randomBounceFactor, randomBounceFactor);
        direction = direction.normalized;
    }

    private void IncrementSpeed()
    {
        speed *= SPEED_INCREASE_FACTOR;
    }

    private void HandlePowerUpCollision(GameObject other)
    {
        IPowerUp powerUp = other.GetComponent<IPowerUp>();
        if (powerUp != null)
        {
            powerUp.ApplyEffect(this);
            Destroy(other);
        }
    }

    private IEnumerator RevertSizeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        size -= SIZE_INCREMENT;
        transform.localScale = new Vector3(size, size, size);
    }
}
