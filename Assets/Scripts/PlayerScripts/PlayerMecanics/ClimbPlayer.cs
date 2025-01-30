using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbing : MonoBehaviour
{
    [Header("Climbing Settings")]
    [SerializeField] private LayerMask climbLayer;
    [SerializeField] private float climbSpeed = 3f;
    //[SerializeField] private float maxClimbTime = 5f;
    [SerializeField] private float exitJumpForce = 8f;
    [SerializeField] private float exitCooldown = 0.5f;
    //[SerializeField] private Image statusClimbBar;

    [Header("Surface Detection")]
    [SerializeField] private float surfaceDetectionDistance = 0.5f;
    [SerializeField] private float cornerCheckRadius = 0.4f;
    [SerializeField] private int cornerRayCount = 8;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 10f;

    private Quaternion targetRotation;

    private CharacterController controller;
    private Movement movementScript;
    private Hang hangScritp;
    private bool isClimbing;
    //private float climbTimer;
    private float cooldownTimer;
    private bool canClimbAgain = true;
    private Vector3 currentSurfaceNormal;
    private Vector3 lastValidPosition;

    private List<Vector3> surfaceHistory = new List<Vector3>();

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        movementScript = GetComponent<Movement>();
        hangScritp = GetComponent<Hang>();
    }

    private void Update()
    {
        HandleCooldown();

        if (!isClimbing && canClimbAgain)
        {
            CheckForClimbableSurface();
        }
        else if (isClimbing)
        {
            HandleClimbing();
        }
    }

    private void HandleCooldown()
    {
        if (!canClimbAgain)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0) canClimbAgain = true;
        }
    }

    private void CheckForClimbableSurface()
    {
        Vector3[] checkDirections = { transform.forward, -transform.forward, transform.right, -transform.right, transform.up, -transform.up };

        foreach (Vector3 direction in checkDirections)
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, surfaceDetectionDistance, climbLayer))
            {
                StartClimbing(hit.normal);
                break;
            }
        }
    }

    private void StartClimbing(Vector3 surfaceNormal)
    {
        isClimbing = true;
        movementScript.DesativateGrabandCombat();
        //climbTimer = maxClimbTime;
        movementScript.enabled = false;
        hangScritp.enabled = false;
        currentSurfaceNormal = surfaceNormal;
        lastValidPosition = transform.position;

        //statusClimbBar.fillAmount = 1f;
        targetRotation = Quaternion.LookRotation(-surfaceNormal, Vector3.up);
        transform.rotation = targetRotation;
    }

    private void HandleClimbing()
    {
        //climbTimer -= Time.deltaTime;
        //statusClimbBar.fillAmount = climbTimer / maxClimbTime;

        if (Input.GetKeyDown(KeyCode.E)) /* climbTimer <= 0 ||*/
        {
            StopClimbing();
            return;
        }

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (Vector3.Dot(currentSurfaceNormal, Vector3.up) > 0.9f)
        {
            StopClimbing();
            return;
        }

        Vector3 moveDirection = CalculateClimbingMoveDirection(horizontal, vertical);

        Vector3 newPosition = transform.position;
        if (CheckAndUpdateSurface(ref newPosition, moveDirection))
        {
            Vector3 movement = newPosition - transform.position;
            controller.Move(movement);
            lastValidPosition = newPosition;
        }
        else
        {
            controller.Move(lastValidPosition - transform.position);
        }

        if (Vector3.Dot(currentSurfaceNormal, Vector3.up) < 0.9f &&
            Vector3.Dot(currentSurfaceNormal, Vector3.down) < 0.9f)
        {
            targetRotation = Quaternion.LookRotation(-currentSurfaceNormal, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private bool CheckAndUpdateSurface(ref Vector3 position, Vector3 moveDirection)
    {
        bool foundSurface = false;
        float closestDistance = float.MaxValue;
        Vector3 newNormal = currentSurfaceNormal;
        Vector3 targetPosition = position + moveDirection * climbSpeed * Time.deltaTime;

        Vector3[] directions = {
        -currentSurfaceNormal,
        currentSurfaceNormal,
        Vector3.Cross(currentSurfaceNormal, Vector3.up),
        Vector3.Cross(currentSurfaceNormal, Vector3.forward),
        Vector3.Cross(Vector3.up, currentSurfaceNormal),
        Vector3.Cross(Vector3.forward, currentSurfaceNormal)
    };

        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit hit;
            if (Physics.SphereCast(targetPosition, cornerCheckRadius * 0.5f, directions[i], out hit, surfaceDetectionDistance * 2f, climbLayer))
            {
                float distance = hit.distance;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    newNormal = hit.normal;
                    position = hit.point + hit.normal * surfaceDetectionDistance;
                    foundSurface = true;
                }
            }
        }

        if (foundSurface)
        {
            if (!surfaceHistory.Contains(newNormal))
            {
                surfaceHistory.Add(newNormal);
            }
            currentSurfaceNormal = newNormal;
            return true;
        }
        else if (surfaceHistory.Count > 0)
        {
            currentSurfaceNormal = surfaceHistory[surfaceHistory.Count - 1];
            surfaceHistory.RemoveAt(surfaceHistory.Count - 1);
            return false;
        }

        return false;
    }


    private Vector3 CalculateClimbingMoveDirection(float horizontal, float vertical)
    {
        Vector3 surfaceUp, surfaceRight;

        if (Mathf.Abs(Vector3.Dot(currentSurfaceNormal, Vector3.up)) > 0.9f)
        {
            surfaceRight = Vector3.ProjectOnPlane(transform.right, currentSurfaceNormal).normalized;
            surfaceUp = Vector3.Cross(currentSurfaceNormal, surfaceRight).normalized;
        }
        else if (Mathf.Abs(Vector3.Dot(currentSurfaceNormal, Vector3.forward)) > 0.9f ||
                 Mathf.Abs(Vector3.Dot(currentSurfaceNormal, Vector3.right)) > 0.9f)
        {
            surfaceUp = Vector3.ProjectOnPlane(Vector3.up, currentSurfaceNormal).normalized;
            surfaceRight = Vector3.Cross(currentSurfaceNormal, surfaceUp).normalized;
        }
        else
        {
            surfaceUp = Vector3.ProjectOnPlane(Vector3.up, currentSurfaceNormal).normalized;
            surfaceRight = Vector3.Cross(currentSurfaceNormal, surfaceUp).normalized;
        }

        Vector3 moveDirection = (surfaceUp * vertical + surfaceRight * horizontal).normalized;

        moveDirection = Vector3.Slerp(moveDirection, currentSurfaceNormal * 0.1f, 0.2f);

        return moveDirection;
    }

    private void StopClimbing()
    {
        isClimbing = false;
        movementScript.enabled = true;
        hangScritp.enabled = true;
        StartCoroutine(ApplyExitForce(-currentSurfaceNormal, exitJumpForce));
        movementScript.AtivateGrabandCombat();
        canClimbAgain = false;
        cooldownTimer = exitCooldown;

        //statusClimbBar.fillAmount = 0f;

    }

    private IEnumerator ApplyExitForce(Vector3 direction, float force)
    {
        float exitDuration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < exitDuration)
        {
            controller.Move(direction * force * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, cornerCheckRadius);

        if (isClimbing)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, currentSurfaceNormal * surfaceDetectionDistance);

            Gizmos.color = Color.blue;
            Vector3[] predefinedDirections = {
                -currentSurfaceNormal,
                currentSurfaceNormal,
                Vector3.Cross(currentSurfaceNormal, Vector3.up),
                Vector3.Cross(currentSurfaceNormal, Vector3.forward),
                Vector3.Cross(Vector3.up, currentSurfaceNormal),
                Vector3.Cross(Vector3.forward, currentSurfaceNormal)
            };

            for (int i = 0; i < cornerRayCount; i++)
            {
                Vector3 direction;
                if (i < predefinedDirections.Length)
                {
                    direction = predefinedDirections[i];
                }
                else
                {
                    float angle = (i - predefinedDirections.Length) * (360f / (cornerRayCount - predefinedDirections.Length));
                    direction = Quaternion.AngleAxis(angle, currentSurfaceNormal) * Vector3.up;
                }

                Gizmos.DrawRay(transform.position, direction * surfaceDetectionDistance);
            }

        }
    }
}