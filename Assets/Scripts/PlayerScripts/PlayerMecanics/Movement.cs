using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeedBase = 4f;
    [SerializeField] private float moveSpeedMax = 8f;
    [SerializeField] private float moveSpeed;
    //[SerializeField] private float accelerationTime = 2f;
    //[SerializeField] private float decelerationTime = 2f;
    //private float currentAccelerationTime = 0f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravity = -30f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Transform cameraTransform;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private float groundedOffset = -0.14f;
    [SerializeField] private Vector3 boxSize = new Vector3(0.5f, 0.1f, 0.5f);
    [SerializeField] private LayerMask groundLayers;

    [Header("Jump Damage")]
    [SerializeField] private float bounceForce = 5f;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Wall Jumping")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private float wallJumpForce = 10f;
    [SerializeField] private Vector3 wallJumpDirection = new Vector3(1f, 1f, 0f);
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private float timeToDefaultLayer = 0.5f;
    [SerializeField] private float timeToClimbableLayer = 1f;
    private bool isWallClimbing;

    [Header("Sliding on Enemy")]
    [SerializeField] private float slideForce = 5f;
    [SerializeField] private float slideDuration = 0.5f;
    private bool isSliding;

    private Combat combatScript;
    private Grab grabScript;
    [SerializeField] private GameObject hand;

    private CharacterController controller;
    [SerializeField] private Animator animator;
    private Vector3 velocity;
    private float turnSmoothVelocity;
    public bool isGrounded;
    private bool wasGroundedLastFrame;
    private float lastGroundedTime;
    private const float COYOTE_TIME = 0.15f;
    private bool isTouchingWall;
    private int wallDirX;
    private GameObject currentWall;
    private GameObject currentPlatform;
    private Vector3 lastPlatformPosition;
    private bool isOnPlatform;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        grabScript = GetComponent<Grab>();
        combatScript = GetComponent<Combat>();
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
        wasGroundedLastFrame = isGrounded;

        Vector3 boxCenter = transform.position + Vector3.up * groundedOffset;
        isGrounded = Physics.BoxCast(
            boxCenter,
            boxSize / 2,
            Vector3.down,
            out RaycastHit hit,
            transform.rotation,
            groundCheckDistance,
            groundLayers,
            QueryTriggerInteraction.Ignore
        );

        if (isGrounded) lastGroundedTime = Time.time;

        if (isGrounded && hit.collider.CompareTag("MovablePlatform"))
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

    private void CheckWallState()
    {
        Vector3 boxCenter = transform.position + Vector3.up * (controller.height / 2);

        isTouchingWall = Physics.BoxCast(
            boxCenter,
            new Vector3(0.5f, 1f, 0.5f),
            transform.forward,
            out RaycastHit hit,
            transform.rotation,
            wallCheckDistance,
            wallLayer
        );

        if (isTouchingWall)
        {
            currentWall = hit.collider.gameObject;
            Vector3 hitDirection = -hit.normal;
            wallDirX = hitDirection.x < 0 ? -1 : 1;
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

            animator.SetFloat("Velocity", moveDir.magnitude);
        }
        else
        {
            animator.SetFloat("Velocity", 0f);
        }
    }
    private void HandleRun()
    {
        if (isGrounded) { moveSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeedMax : moveSpeedBase; }

        //if (isGrounded) 
        //{
        //    //if (isGrounded) { moveSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeedMax : moveSpeedBase; }

        //    bool isRunning = Input.GetKey(KeyCode.LeftShift);
        //    //Debug.Log("shift");
        //    currentAccelerationTime = isRunning ? Mathf.Min(currentAccelerationTime + Time.deltaTime / accelerationTime, 1f)
        //                              : Mathf.Max(currentAccelerationTime - Time.deltaTime / decelerationTime, 0f);

        //    moveSpeed = Mathf.Lerp(moveSpeedBase, moveSpeedMax, currentAccelerationTime);

        //}
    }
    public void ResetSpeed() { moveSpeed = moveSpeedBase; }

    private void HandleJump()
    {
        bool canJump = isGrounded || (Time.time - lastGroundedTime <= COYOTE_TIME);

        if (Input.GetButtonDown("Jump") && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            lastGroundedTime = 0f;
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
            velocity.y = -wallSlideSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                Vector3 jumpDirection = wallJumpDirection.normalized;
                jumpDirection.x *= wallDirX;
                jumpDirection = jumpDirection.normalized;

                velocity = jumpDirection * wallJumpForce;
                velocity.y = Mathf.Sqrt(wallJumpForce * -2f * gravity);

                StartCoroutine(RestoreHorizontalVelocity());
                StartCoroutine(RestoreWallLayer());
            }
            else
            {
                velocity.y = -wallSlideSpeed;
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
            yield return new WaitForSeconds(timeToDefaultLayer);
            wallMoment.layer = LayerMask.NameToLayer("Climbable");
            yield return new WaitForSeconds(timeToClimbableLayer);
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
            if (Physics.BoxCast(transform.position, boxSize / 2, Vector3.down, out RaycastHit hit, transform.rotation, raycastDistance, enemyLayer))
            {
                if (hit.collider.gameObject.CompareTag("Sapo"))
                {
                    sapo sapoScript = hit.collider.gameObject.GetComponent<sapo>();
                    if (sapoScript.damage) { PlayerLife playerlife = GetComponent<PlayerLife>(); playerlife.TakeDamage(1); }
                    if (sapoScript.isInflating == true) velocity.y = bounceForce;
                    else StartCoroutine(SlideOffEnemy());
                }
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    BodyDestroy bodyDestroy = hit.collider.gameObject.GetComponent<BodyDestroy>();
                    if (bodyDestroy != null)
                    {
                        velocity.y = bounceForce;
                        bodyDestroy.DamageInHead();
                    }
                }
                if (hit.collider.gameObject.CompareTag("Gorilla"))
                {
                    BossMovement bossMovement = hit.collider.gameObject.GetComponent<BossMovement>();
                    GorillaLife gorillaLife = hit.collider.GetComponent<GorillaLife>();
                    if (bossMovement != null && gorillaLife != null)
                    {
                        bossMovement.OnHeadJump();
                        gorillaLife.TakeDamage(1);
                    }
                }
            }
        }
    }

    private IEnumerator SlideOffEnemy()
    {
        isSliding = true;

        Vector3 slideDirection = transform.right;

        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / slideDuration;

            controller.Move(slideDirection * slideForce * Time.deltaTime);

            yield return null;
        }

        isSliding = false;
    }

    public void DesativateGrabandCombat()
    {
        grabScript.enabled = false;
        combatScript.enabled = false;
        hand.SetActive(false);
    }

    public void AtivateGrabandCombat()
    {
        grabScript.enabled = true;
        combatScript.enabled = true;
        hand.SetActive(true);
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        float maxFallSpeed = -30f;
        if (velocity.y < maxFallSpeed)
        {
            velocity.y = maxFallSpeed;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        Gizmos.color = isGrounded ? transparentGreen : transparentRed;

        Vector3 boxCenter = transform.position + Vector3.up * groundedOffset;
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.down * (groundCheckDistance / 2), boxSize);
        Gizmos.DrawCube(Vector3.down * (groundCheckDistance / 2), boxSize);
    }

    public void Jump(float jumpForceDetach)
    {
        velocity.y = Mathf.Sqrt(jumpForceDetach * -2f * gravity);
    }

    public void JumpForward(float jumpForceDetach, float forwardForce)
    {
        velocity.y = Mathf.Sqrt(jumpForceDetach * -2f * gravity);

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward = cameraForward.normalized;

        float targetAngle = Mathf.Atan2(cameraForward.x, cameraForward.z) * Mathf.Rad2Deg;
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        velocity += moveDir * forwardForce;

        StartCoroutine(ResetForwardMomentum());
    }

    private IEnumerator ResetForwardMomentum()
    {
        yield return new WaitForSeconds(0.5f);

        float resetDuration = 0.3f;
        float elapsedTime = 0f;
        Vector3 initialVelocity = new Vector3(velocity.x, 0f, velocity.z);

        while (elapsedTime < resetDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / resetDuration;

            velocity.x = Mathf.Lerp(initialVelocity.x, 0f, t);
            velocity.z = Mathf.Lerp(initialVelocity.z, 0f, t);

            yield return null;
        }

        velocity.x = 0f;
        velocity.z = 0f;
    }

    public void StartCenterPoint(Transform centerPoint)
    {
        StartCoroutine(ThrowToCenterPoint(centerPoint));
    }

    IEnumerator ThrowToCenterPoint(Transform centerPoint)
    {
        Jump(2);
        yield return new WaitForSeconds(0.5f);
        Vector3 start = transform.position;
        float time = 0f;
        float throwDuration = 1f;

        while (time < throwDuration)
        {
            time += Time.deltaTime;
            float t = time / throwDuration;

            transform.position = Vector3.Lerp(start, centerPoint.position, t);
            transform.position += Vector3.up * Mathf.Sin(t * Mathf.PI) * 2f;

            yield return null;
        }

        transform.position = centerPoint.position;
    }
}