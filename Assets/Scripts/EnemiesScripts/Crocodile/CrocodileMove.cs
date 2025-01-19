using UnityEngine;

public class CrocodileMove : MonoBehaviour
{
    [Header("-----Move-----")]
    [SerializeField] GameObject[] waypoints;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;

    private int currentWaypoint = 0;
    private float initialSpeed;

    private void Start()
    {
        initialSpeed = maxSpeed;
    }

    private void Update()
    {
        CalculateDistance();
        MoveToWaypoint();
    }

    private void MoveToWaypoint()
    {
        maxSpeed = initialSpeed;
        if (waypoints != null && waypoints.Length > 0)
        {
            Vector3 direction = (waypoints[currentWaypoint].transform.position - transform.position).normalized;
            transform.Translate(direction * maxSpeed * Time.deltaTime, Space.World);
            RotateRute(direction);
        }
    }

    private void RotateRute(Vector3 newDirection)
    {
        if (waypoints != null && waypoints.Length > 0 && currentWaypoint >= 0 && currentWaypoint < waypoints.Length)
        {
            newDirection = waypoints[currentWaypoint].transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(newDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    private void CalculateDistance()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 1f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }
    }
}
