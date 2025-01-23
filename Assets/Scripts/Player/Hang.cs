using UnityEngine;

public class Hang : MonoBehaviour
{
    private GameObject target;
    private bool isStuck = false;
    private bool canDetect = false;
    private CharacterController characterController;
    private Movement movementScripts;
    [SerializeField] private float jumpForce = 5f;
    private bool canJump = false;

    [SerializeField] private BoxCollider lianaMovementBox;

    [SerializeField] private float moveSpeed = 5f; 

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        movementScripts = GetComponent<Movement>();
    }

    void Update()
    {
        if (isStuck)
        {
            if (target == null)
            {
                ReleaseFromLiana();
                return;
            }

            Vector3 lianaPosition = target.transform.position;
            float yMin = lianaMovementBox.bounds.min.y;
            float yMax = lianaMovementBox.bounds.max.y;
            float clampedY = Mathf.Clamp(transform.position.y, yMin, yMax);
            transform.position = new Vector3(lianaPosition.x, clampedY, lianaPosition.z);

            MoveWhileHanging();
        }

        HangMonkey();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Liana"))
        {
            target = other.gameObject;
            canDetect = true;
            lianaMovementBox = target.GetComponent<BoxCollider>();
        }
    }

    void HangMonkey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canDetect)
            {
                if (target == null) return;

                characterController.enabled = false;
                movementScripts.enabled = false;
                transform.position = target.transform.position;
                isStuck = true;
                canDetect = false;
                canJump = true;
            }
            else
            {
                ReleaseFromLiana();

                if (canJump)
                {
                    Jump();
                    canJump = false;
                }
            }
        }
    }

    private void ReleaseFromLiana()
    {
        characterController.enabled = true;
        movementScripts.enabled = true;
        isStuck = false;
    }

    private void Jump()
    {
        movementScripts.Jump(jumpForce);
    }

    private void MoveWhileHanging()
    {
        float verticalInput = Input.GetAxis("Vertical"); 
        float newY = transform.position.y + verticalInput * moveSpeed * Time.deltaTime;

        float yMin = lianaMovementBox.bounds.min.y;
        float yMax = lianaMovementBox.bounds.max.y;
        newY = Mathf.Clamp(newY, yMin, yMax);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}