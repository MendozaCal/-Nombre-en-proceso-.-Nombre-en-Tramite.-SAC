using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatrol : MonoBehaviour
{
    [Header("-----Move-----")]
    public Transform[] waypoints;
    public float maxSpeed = 10;
    public int currentWaypoint = 0;
    protected float inicialSpeed;
    private bool movingForward = true; // Controla la dirección del movimiento

    [Header("----References----")]
    public EnemyLife enemyLife;
    private void Start()
    {
        inicialSpeed = maxSpeed;
    }

    protected virtual void Update()
    {
        if (enemyLife.isDead) return;
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
            if (movingForward)
            {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Length)
                {
                    currentWaypoint = waypoints.Length - 2; // Retrocede al penúltimo punto
                    movingForward = false;
                }
            }
            else
            {
                currentWaypoint--;
                if (currentWaypoint < 0)
                {
                    currentWaypoint = 1; // Avanza al segundo punto
                    movingForward = true;
                }
            }
        }
    }
}