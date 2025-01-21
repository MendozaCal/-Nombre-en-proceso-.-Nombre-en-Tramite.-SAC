using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspMove : MonoBehaviour
{
    [Header("----- Move -----")]
    [SerializeField] GameObject[] waypoints;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float waitTimeAtWaypoint = 2f;
    private GameObject player;

    private int currentWaypoint = 0;
    private float initialSpeed;
    private bool isWaiting = false;
    private bool movingForward = true;

    private void Start()
    {
        player = GameObject.Find("Player");
        initialSpeed = maxSpeed;
    }

    private void Update()
    {
        RotateRute();

        if (!isWaiting)
        {
            MoveToWaypoint();
        }
    }

    private void MoveToWaypoint()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            maxSpeed = initialSpeed;
            Vector3 direction = (waypoints[currentWaypoint].transform.position - transform.position).normalized;
            transform.Translate(direction * maxSpeed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 1f)
            {
                StartCoroutine(WaitAtWaypoint());
            }
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;

        yield return new WaitForSeconds(waitTimeAtWaypoint);

        if (movingForward)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = waypoints.Length - 1;
                movingForward = false;
            }
        }
        else
        {
            currentWaypoint--;
            if (currentWaypoint < 0)
            {
                currentWaypoint = 0;
                movingForward = true;
            }
        }

        isWaiting = false;
    }

    private void RotateRute()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
