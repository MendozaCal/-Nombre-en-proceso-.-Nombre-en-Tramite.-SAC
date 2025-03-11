using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatrol : MonoBehaviour
{
    [Header("-----Move-----")]
    public Transform[] waypoints;
    public float maxSpeed = 10;
    public int currentWaypoint = 0;
    public bool isCave;
    protected float inicialSpeed;
    private void Start()
    {
        inicialSpeed = maxSpeed;
    }

    protected virtual void Update()
    {
        MoveToWaypoint();    
        calculateDistance();
    }
    protected virtual void MoveToWaypoint()
    {
        Vector3 direction = waypoints[currentWaypoint].position - transform.position;
        direction.Normalize();
        transform.Translate(direction * maxSpeed * Time.deltaTime, Space.World);        
    }
    protected virtual void calculateDistance()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 1)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }
    }
}