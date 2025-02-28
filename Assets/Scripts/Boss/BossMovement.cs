using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] barrel;
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private float waitBetweenJumps = 1f;
    [SerializeField] private float heightJump = 2f;
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Movement movement;

    private bool attckGorilla;
    private bool activeAttack;
    private bool isStunned = false;
    private int currentIndex = 0;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float elapsedTime;

    void Start()
    {
        StartCoroutine(MovimientoCiclo());
    }

    void Update()
    {
        if (isStunned) return;

        if (attckGorilla)
        {
            RotateatAtPlayer();
            if (!activeAttack) StartCoroutine(AttackGorillaLaunch());
        }
    }

    IEnumerator MovimientoCiclo()
    {
        while (true)
        {
            if (isStunned)
            {
                yield return null;
                continue;
            }

            Transform objetive = points[currentIndex];

            startPosition = transform.position;
            targetPosition = objetive.position;
            elapsedTime = 0f;

            while (elapsedTime < jumpDuration)
            {
                if (isStunned) break;

                elapsedTime += Time.deltaTime;
                float t = elapsedTime / jumpDuration;

                Vector3 direction = (targetPosition - transform.position).normalized;
                direction.y = 0;

                Quaternion rotationFinal = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotationFinal, 0.2f);

                Vector3 intermediatePosition = Vector3.Lerp(startPosition, targetPosition, t);

                intermediatePosition.y += Mathf.Sin(t * Mathf.PI) * heightJump;

                transform.position = intermediatePosition;
                yield return null;
            }

            if (!isStunned)
            {
                transform.position = targetPosition;

                attckGorilla = true;
                yield return new WaitForSeconds(waitBetweenJumps);
                attckGorilla = false;

                currentIndex = (currentIndex + 1) % points.Length;
            }
        }
    }

    void RotateatAtPlayer()
    {
        if (isStunned) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    IEnumerator AttackGorillaLaunch()
    {
        if (isStunned) yield break;

        activeAttack = true;

        Vector3 targetPosition = player.position;
        float distance = Vector3.Distance(transform.position, targetPosition);

        float maxHeight = CalculateAdaptiveHeight(distance);

        (Vector3 initialVelocity, float totalTime) = CalculatePreciseTrajectory(transform.position, targetPosition, maxHeight);

        GameObject obj = Instantiate(GetRandomBarrel(), transform.position, Quaternion.identity);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = initialVelocity;

        yield return new WaitForSeconds(totalTime);
        activeAttack = false;
    }

    float CalculateAdaptiveHeight(float distance)
    {
        if (distance < 5f) return 1f;
        if (distance < 10f) return 2f;
        if (distance < 20f) return 4f;
        return 5f;
    }

    (Vector3, float) CalculatePreciseTrajectory(Vector3 startPos, Vector3 targetPos, float maxHeight)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);

        float horizontalDistance = Vector2.Distance(
            new Vector2(startPos.x, startPos.z),
            new Vector2(targetPos.x, targetPos.z)
        );

        float timeToApex = Mathf.Sqrt(2 * maxHeight / gravity);
        float timeToTarget = 2 * timeToApex;

        Vector3 horizontalDirection = (targetPos - startPos).normalized;
        float horizontalSpeed = horizontalDistance / timeToTarget;
        float verticalSpeed = Mathf.Sqrt(2 * gravity * maxHeight);

        Vector3 initialVelocity = new Vector3(
            horizontalDirection.x * horizontalSpeed,
            verticalSpeed,
            horizontalDirection.z * horizontalSpeed
        );

        return (initialVelocity, timeToTarget);
    }

    public void StartStun()
    {
        StartCoroutine(StunBoss());
    }

    IEnumerator StunBoss()
    {
        isStunned = true;
        Vector3 currentPosition = transform.position;

        int nearestPointIndex = FindNearestPointIndex(currentPosition);
        currentIndex = nearestPointIndex;
        targetPosition = points[currentIndex].position;

        float quickMoveDuration = 0.5f;
        float elapsedQuickMoveTime = 0f;
        Vector3 stunStartPosition = currentPosition;

        while (elapsedQuickMoveTime < quickMoveDuration)
        {
            elapsedQuickMoveTime += Time.deltaTime;
            float t = elapsedQuickMoveTime / quickMoveDuration;

            float smoothT = 1 - (1 - t) * (1 - t);

            transform.position = Vector3.Lerp(stunStartPosition, targetPosition, smoothT);
            yield return null;
        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(stunDuration - quickMoveDuration);

        startPosition = targetPosition;
        elapsedTime = 0f;
        isStunned = false;
    }

    private int FindNearestPointIndex(Vector3 position)
    {
        int nearestIndex = 0;
        float nearestDistance = float.MaxValue;

        int nextIndex = (currentIndex + 1) % points.Length;
        float distanceToCurrent = Vector3.Distance(position, points[currentIndex].position);
        float distanceToNext = Vector3.Distance(position, points[nextIndex].position);

        if (distanceToCurrent <= distanceToNext)
        {
            return currentIndex;
        }
        else
        {
            return nextIndex;
        }
    }

    public void OnHeadJump()
    {
        isStunned = false;
        movement.StartCenterPoint(centerPoint);
    }

    private GameObject GetRandomBarrel()
    {
        float totalWeight = 45f + 15f + 20f + 20f;
        float randomValue = Random.Range(0, totalWeight);

        if (randomValue <= 45f) return barrel[0];
        else if (randomValue <= 60f) return barrel[1];
        else if (randomValue <= 80f) return barrel[2];
        else return barrel[3];
    }
}