using System.Collections;
using UnityEngine;
public class WallClimbing : MonoBehaviour
{
    [Header("Climbing Settings")]
    [SerializeField] private LayerMask climbLayer;
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private float maxClimbTime = 5f;
    [SerializeField] private float exitJumpForce = 8f;
    [SerializeField] private float exitCooldown = 0.5f;
    [SerializeField] private float backwardExitForce = 5f; 

    private CharacterController controller;
    private Movement movementScript;
    private bool isClimbing;
    private float climbTimer;
    private float cooldownTimer;
    private bool canClimbAgain = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        movementScript = GetComponent<Movement>();
    }

    private void Update()
    {
        if (!canClimbAgain)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                canClimbAgain = true;
            }
        }

        if (!isClimbing && canClimbAgain && CanClimb())
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
        if (!canClimbAgain) return false;

        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * (controller.height / 2);
        bool canClimb = Physics.Raycast(origin, transform.forward, out hit, 1f, climbLayer);
        return canClimb;
    }

    private void StartClimbing()
    {
        isClimbing = true;
        climbTimer = maxClimbTime;
        movementScript.enabled = false;
    }

    private void HandleClimbing()
    {
        climbTimer -= Time.deltaTime;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 climbDirection = new Vector3(horizontal, vertical, 0f);

        if (vertical > 0.5f && IsNearTopEdge())
        {
            ExitClimbingUpward();
            return;
        }

        if (climbTimer <= 0 || Input.GetKeyDown(KeyCode.Space))
        {
            ExitClimbingBackward();
            return;
        }

        if (!CanClimb())
        {
            ExitClimbingBackward();
            return;
        }

        controller.Move(climbDirection * climbSpeed * Time.deltaTime);
    }

    private bool IsNearTopEdge()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * (controller.height - 0.2f);
        return !Physics.Raycast(origin, transform.forward, out hit, 1f, climbLayer);
    }

    private void ExitClimbingUpward()
    {
        isClimbing = false;
        movementScript.enabled = true;

        Vector3 jumpDirection = (Vector3.up * 2f + transform.forward).normalized;
        StartCoroutine(ApplyExitForce(jumpDirection, exitJumpForce));

        canClimbAgain = false;
        cooldownTimer = exitCooldown;
    }

    private void ExitClimbingBackward()
    {
        isClimbing = false;
        movementScript.enabled = true;

        Vector3 jumpDirection = (-transform.forward + Vector3.up * 0.5f).normalized;
        StartCoroutine(ApplyExitForce(jumpDirection, backwardExitForce));

        canClimbAgain = false;
        cooldownTimer = exitCooldown;
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
        if (controller == null) return;
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up * (controller.height / 2);
        Gizmos.DrawLine(origin, origin + transform.forward * 1f);

        Gizmos.color = Color.yellow;
        Vector3 topOrigin = transform.position + Vector3.up * (controller.height - 0.2f);
        Gizmos.DrawLine(topOrigin, topOrigin + transform.forward * 1f);
    }
}