using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    public float waitTime = 1f;

    private int currentWaypointIndex = 0; 
    private bool movingForward = true;
    private float waitCounter = 0f; 

    private Vector3 previousPosition; 
    private bool playerOnPlatform = false; 
    private Transform playerTransform; 
    private void Start()
    {
        previousPosition = transform.position;
    }

    private void Update()
    {
        if (waypoints.Length < 2) return;

        if (waitCounter > 0)
        {
            waitCounter -= Time.deltaTime;
            return;
        }


        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            waitCounter = waitTime;
            currentWaypointIndex = movingForward ? currentWaypointIndex + 1 : currentWaypointIndex - 1;

            if (currentWaypointIndex >= waypoints.Length)
            {
                movingForward = false;
                currentWaypointIndex = waypoints.Length - 2;
            }
            else if (currentWaypointIndex < 0)
            {
                movingForward = true;
                currentWaypointIndex = 1;
            }
        }

        if (playerOnPlatform && playerTransform != null)
        {
            Vector3 platformMovement = transform.position - previousPosition;
            playerTransform.position += platformMovement;
        }


        previousPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player ha entrado al trigger de la plataforma.");
            playerOnPlatform = true;
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player ha salido del trigger de la plataforma.");
            playerOnPlatform = false;
            playerTransform = null;
        }
    }
}
