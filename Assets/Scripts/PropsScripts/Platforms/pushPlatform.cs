using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushPlatform : MonoBehaviour
{
    [SerializeField] Transform endPoint;
    [SerializeField] bool isNotRandom;
    [SerializeField] float speed = 2f;
    [SerializeField] float cooldownTime = 1f;
    private Vector3 startPoint;
    private Vector3 targetPosition;
    private bool canMove = true;

    void Start()
    {
        startPoint = transform.position;
        targetPosition = endPoint.position;
        if (!isNotRandom)
        {
            speed = Random.Range(2f,5f);
            cooldownTime = Random.Range(0.5f,1f);
        }
    }

    void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                StartCoroutine(ChangeDirectionWithCooldown());
            }
        }
    }

    IEnumerator ChangeDirectionWithCooldown()
    {
        canMove = false;
        yield return new WaitForSeconds(cooldownTime);
        targetPosition = (targetPosition == startPoint) ? endPoint.position : startPoint;
        canMove = true;
    }
}
