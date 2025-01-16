using System.Collections;
using UnityEngine;

public class WallClimbing : MonoBehaviour
{
    [Header("Climbing Settings")]
    [SerializeField] private LayerMask climbLayer;
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private float maxClimbTime = 5f;

    private CharacterController controller;
    private Movement movementScript;
    private bool isClimbing;
    private float climbTimer;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        movementScript = GetComponent<Movement>();
    }

    private void Update()
    {
        if (CanClimb())
        {
            StartClimbing();
        }

        if (isClimbing)
        {
            HandleClimbing();
        }
    }

    private bool CanClimb()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * (controller.height / 2);
        bool canClimb = Physics.Raycast(origin, transform.forward, out hit, 1f, climbLayer);
        return canClimb;
    }

    private void StartClimbing()
    {
        isClimbing = true;
        climbTimer = maxClimbTime;
        movementScript.enabled = false; // Disable normal movement
    }

    private void HandleClimbing()
    {
        climbTimer -= Time.deltaTime;
        if (!CanClimb() || climbTimer <= 0 || Input.GetKeyDown(KeyCode.Space))
        {
            StopClimbing();
            return;
        }

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 climbDirection = new Vector3(horizontal, vertical, 0f);
        controller.Move(climbDirection * climbSpeed * Time.deltaTime);
    }

    private void StopClimbing()
    {
        isClimbing = false;
        movementScript.enabled = true; // Re-enable normal movement
    }

    private void OnDrawGizmos()
    {
        if (controller == null) return;

        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up * (controller.height / 2);
        Gizmos.DrawLine(origin, origin + transform.forward * 1f);
    }
}
