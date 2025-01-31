using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject barrel;
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private float waitBetweenJumps = 1f;
    [SerializeField] private float heightJump = 2f;
    [SerializeField] private float stunDuration = 2f;

    private bool attckGorilla;
    private bool activeAttack;
    private bool isStunned = false;
    private int currentIndex = 0;

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

            Vector3 start = transform.position;
            float time = 0f;

            while (time < jumpDuration)
            {
                if (isStunned) break;

                time += Time.deltaTime;
                float t = time / jumpDuration;

                Vector3 direction = (objetive.position - transform.position).normalized;
                direction.y = 0;

                Quaternion rotationFinal = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotationFinal, 0.2f);

                Vector3 intermediatePosition = Vector3.Lerp(start, objetive.position, t);

                intermediatePosition.y += Mathf.Sin(t * Mathf.PI) * heightJump;

                transform.position = intermediatePosition;
                yield return null;
            }

            if (!isStunned)
            {
                transform.position = objetive.position;

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

        GameObject obj = Instantiate(barrel, transform.position, Quaternion.identity);
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
        Debug.Log("Fui estuneado");
        yield return new WaitForSeconds(stunDuration);
        Debug.Log("Ya no estoy estuneado");
        isStunned = false;
    }
}