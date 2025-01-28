using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeedBase = 4f;
    [SerializeField] private float moveSpeedMax = 8f;
    private float moveSpeed;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravity = -30f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Transform cameraTransform;

    [Header("Jump Damage")]
    [SerializeField] private float bounceForce = 5f;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Vector3 boxSize = new Vector3(0.5f, 0.1f, 0.5f);
    [SerializeField] private float groundCheckDistance;

    [Header("Wall Climbing")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private float wallJumpForce = 10f;
    [SerializeField] private Vector3 wallJumpDirection = new Vector3(1f, 1f, 0f);
    [SerializeField] private float wallSlideSpeed = 2f;
    private bool isWallClimbing;

    private CharacterController controller;
    private Vector3 velocity;
    private float turnSmoothVelocity;
    public bool isGrounded;
    private bool isTouchingWall;
    private int wallDirX;
    private GameObject currentWall;

    private GameObject currentPlatform; 
    private Vector3 lastPlatformPosition; 
    private bool isOnPlatform;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        moveSpeed = moveSpeedBase;
    }

    private void Update()
    {
        CheckGroundState();
        CheckWallState();
        HandleMovement();
        HandleRun();
        HandleJump();
        HandleWallMovement();
        CheckEnemyBelow();
        ApplyPlatformMovement();
        ApplyGravity();
    }

    private void CheckGroundState()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

          if (Physics.BoxCast(transform.position, boxSize / 2, Vector3.down, out RaycastHit hit, Quaternion.identity, groundCheckDistance))
        {
            if (hit.collider.CompareTag("MovablePlatform")) 
            {
                isOnPlatform = true;
                if (currentPlatform != hit.collider.gameObject)
                {
                    currentPlatform = hit.collider.gameObject;
                    lastPlatformPosition = currentPlatform.transform.position;
                }
            }
            else
            {
                isOnPlatform = false;
                currentPlatform = null;
            }
        }
        else
        {
            isOnPlatform = false;
            currentPlatform = null;
        }
    }

    private void CheckWallState()
    {
        Vector3 boxCenter = transform.position + Vector3.up * (controller.height / 2); 
        isTouchingWall = Physics.BoxCast(boxCenter, new Vector3(0.5f, 1f, 0.5f), transform.forward, out RaycastHit hit, Quaternion.identity, wallCheckDistance, wallLayer);

        if (isTouchingWall)
        {
            currentWall = hit.collider.gameObject;
            wallDirX = hit.normal.x < 0 ? -1 : 1; 
        }
        else
        {
            wallDirX = 0;
            currentWall = null;
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleRun()
    {
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeedMax : moveSpeedBase;
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    private void ApplyPlatformMovement()
    {
        if (isOnPlatform && currentPlatform != null)
        {
            Vector3 platformDisplacement = currentPlatform.transform.position - lastPlatformPosition;

            controller.Move(platformDisplacement);

            lastPlatformPosition = currentPlatform.transform.position;
        }
    }

    private void HandleWallMovement()
    {
        if (isTouchingWall && !isGrounded)
        {
            if (!isWallClimbing)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    isWallClimbing = true;
                    velocity.y = 0;

                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, wallCheckDistance, wallLayer))
                    {
                        currentWall = hit.collider?.gameObject;
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown("Jump"))
                {
                    Vector3 jumpDirection = wallJumpDirection.normalized;
                    jumpDirection.x *= wallDirX;
                    jumpDirection = jumpDirection.normalized;
                    velocity = jumpDirection * wallJumpForce;
                    velocity.y = Mathf.Sqrt(wallJumpForce * -2f * gravity);
                    StartCoroutine(RestoreHorizontalVelocity());
                    StartCoroutine(RestoreWallLayer());
                    isWallClimbing = false;
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    velocity.y = -wallSlideSpeed * 2;
                    isWallClimbing = false;
                    StartCoroutine(RestoreWallLayer());
                }
                else
                {
                    velocity.y = -wallSlideSpeed;
                }
            }
        }
        else
        {
            isWallClimbing = false;
        }
    }

    private IEnumerator RestoreWallLayer()
    {
        if (currentWall != null)
        {
            GameObject wallMoment = currentWall.gameObject;
            currentWall.layer = LayerMask.NameToLayer("Default");
            yield return new WaitForSeconds(0.75f);
            wallMoment.layer = LayerMask.NameToLayer("Climbable");
            yield return new WaitForSeconds(1f);
            currentWall = null; 
        }
    }

    private IEnumerator RestoreHorizontalVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        velocity.x = 0;
        velocity.z = 0;
    }

    private void CheckEnemyBelow()
    {
        if (velocity.y < 0)
        {
            if (Physics.BoxCast(transform.position, boxSize / 2, Vector3.down, out RaycastHit hit, Quaternion.identity, raycastDistance, enemyLayer))
            {
                if (hit.collider.gameObject.CompareTag("Sapo"))
                {
                    sapo sapoScript = hit.collider.gameObject.GetComponent<sapo>();
                    if (sapoScript.damage) { PlayerLife playerlife = GetComponent<PlayerLife>(); playerlife.TakeDamage(1); }
                    if(sapoScript.isInflating == true) velocity.y = bounceForce;
                }
                if(hit.collider.gameObject.CompareTag("Enemy"))
                {
                    BodyDestroy bodyDestroy = hit.collider.gameObject.GetComponent<BodyDestroy>();
                    if (bodyDestroy != null)
                    {
                        bodyDestroy.PlayerDestroy();
                        velocity.y = bounceForce;
                    }
                }
            }
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckDistance, boxSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * wallCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position - transform.right * wallCheckDistance);
    }

    public void Jump(float jumpForceDetach)
    {
        velocity.y = Mathf.Sqrt(jumpForceDetach * -2f * gravity);
    }
}