using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private Transform player;
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private float waitBetweenJumps = 1f;
    [SerializeField] private float heightJump = 2f;

    private int currentIndex = 0;

    void Start()
    {
        StartCoroutine(MovimientoCiclo());
    }

    IEnumerator MovimientoCiclo()
    {
        while (true)
        {
            Transform objetive = points[currentIndex];

            Vector3 start = transform.position;
            float time = 0f;

            while (time < jumpDuration)
            {
                time += Time.deltaTime;
                float t = time / jumpDuration;

                Vector3 direction = (objetive.position - transform.position).normalized;
                direction.y = 0; 

                Quaternion rotationFinal = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotationFinal, 0.2f);

                Vector3 posicionIntermedia = Vector3.Lerp(start, objetive.position, t);

                posicionIntermedia.y += Mathf.Sin(t * Mathf.PI) * heightJump;

                transform.position = posicionIntermedia;
                yield return null;
            }

            transform.position = objetive.position;

            yield return StartCoroutine(RotateatAtPlayer(waitBetweenJumps));

            currentIndex = (currentIndex + 1) % points.Length;
        }
    }
    IEnumerator RotateatAtPlayer(float tiempo)
    {
        float elapsedTime = 0f;
        while (elapsedTime < tiempo)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}