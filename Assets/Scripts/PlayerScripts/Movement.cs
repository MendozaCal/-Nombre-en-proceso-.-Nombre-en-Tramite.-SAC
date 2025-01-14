using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravity = -30f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Transform cameraTransform;

    [Header("Jump Damage")]
    //[SerializeField] private float groundPoundDamage = 20f;
    [SerializeField] private float bounceForce = 5f; 
    [SerializeField] private float raycastDistance = 1f; 
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Vector3 boxSize = new Vector3(0.5f, 0.1f, 0.5f);
    [SerializeField] private float groundCheckDistance;

    private CharacterController controller;
    private Vector3 velocity;
    private float turnSmoothVelocity;
    private bool isGrounded;

    private void Start()
    {
        InitializeComponents();
    }

    private void Update()
    {
        CheckGroundState();
        HandleMovement();
        HandleJump();
        CheckEnemyBelow();
        ApplyGravity();
    }

    private void InitializeComponents()
    {
        controller = GetComponent<CharacterController>();
    }

    private void CheckGroundState()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void CheckEnemyBelow()
    {
        if (velocity.y < 0)
        {

            if (Physics.BoxCast(transform.position, boxSize / 2, Vector3.down, out RaycastHit hit, Quaternion.identity, raycastDistance, enemyLayer))
            {
                hit.collider.gameObject.GetComponent<TortoiseDead>()?.PlayerDestroy();
                //Metodo en caso del enemigo tener vida y manera de matarlo
                velocity.y = bounceForce;
            }
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = CalculateTargetAngle(direction);
            RotateCharacter(targetAngle);
            MoveCharacter(targetAngle);
        }
    }

    private float CalculateTargetAngle(Vector3 direction)
    {
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
    }

    private void RotateCharacter(float targetAngle)
    {
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void MoveCharacter(float targetAngle)
    {
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
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
    }
}