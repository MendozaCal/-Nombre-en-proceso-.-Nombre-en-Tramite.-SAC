using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortoiseMove : MonoBehaviour
{
    [Header("-----Move-----")]
    [SerializeField] GameObject[] waypoints;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;
    
    [Header("-----Attack-----")]
    [SerializeField] bool isDetected;
    [SerializeField] float durationAttack = 10f;
    [SerializeField] float maxSpeedAttack = 7.5f;

    private int currentWaypoint = 0;
    private float initialSpeed;
    private float initialDurationAttack;
    private GameObject targetDetectorRute;
    private DetectorPatrollToAttack detectorPatrollToAttack;
    private GameObject targetDetectorEyes;
    private EyesDetectorTortoise eyesDetectorTurtle;
    private GameObject targetDetectorArea;
    private AreaPersecution areaDetectorTurtle;
    private GameObject player;

    private void Start()
    {
        targetDetectorRute = GameObject.Find("TurtleRutePoints");
        if (targetDetectorRute != null)
        {
            detectorPatrollToAttack = targetDetectorRute.GetComponent<DetectorPatrollToAttack>();
        }
        targetDetectorEyes = GameObject.Find("Eyes");
        if (targetDetectorEyes != null)
        {
            eyesDetectorTurtle = targetDetectorEyes.GetComponent<EyesDetectorTortoise>();
        }
        targetDetectorArea = GameObject.Find("TurtleController");
        if (targetDetectorEyes != null)
        {
            areaDetectorTurtle = targetDetectorArea.GetComponent<AreaPersecution>();
        }
        player = GameObject.Find("Player");
        initialSpeed = maxSpeed;
        initialDurationAttack = durationAttack;
    }

    private void Update()
    {
        DetectPlayer();
        if (!isDetected)
        {
            MoveToWaypoint();
            CalculateDistance();
        }
        else
        {
            AttackTortoise();
        }
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

    private void DetectPlayer()
    {
        if (detectorPatrollToAttack != null && eyesDetectorTurtle != null)
        {
            if (detectorPatrollToAttack.detectedPlayerRute && eyesDetectorTurtle.detectedPlayerEyes && areaDetectorTurtle.detectedPlayerArea)
            {
                isDetected = true;
            }
            else
            {
                isDetected = false;
                
            }
        }
    }

    private void AttackTortoise()
    {
        if (player != null )
        {
            maxSpeed = maxSpeedAttack;
            durationAttack -= Time.deltaTime;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.Translate(direction * maxSpeed * Time.deltaTime, Space.World);
            LookPlayer(direction);
            if (durationAttack <= 0)
            {
                ReturnPatroll();
            }
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

    void LookPlayer(Vector3 direction)
    {
        direction = player.transform.position - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ReturnPatroll();
        }
    }
    public void ReturnPatroll()
    {
        isDetected = false;
        durationAttack = initialDurationAttack;
        detectorPatrollToAttack.detectedPlayerRute = false;
        eyesDetectorTurtle.detectedPlayerEyes = false;
    }
}