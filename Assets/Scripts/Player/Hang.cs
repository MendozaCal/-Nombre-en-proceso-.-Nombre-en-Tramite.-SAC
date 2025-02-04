using UnityEngine;

public class Hang : MonoBehaviour
{
    private GameObject target;
    private GameObject ignoredLiana;
    private bool isStuck = false;
    private bool canDetect = false;
    private CharacterController characterController;
    private Movement movementScripts;
    [SerializeField] private float jumpForce = 5f;
    private bool canJump = false;

    [SerializeField] private BoxCollider lianaMovementBox;

    [SerializeField] private float ignoreDuration = 1f;
    private float ignoreTimer = 0f;

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
        }

        HangMonkey();
        HandleIgnoreTimer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Liana") && other.gameObject != ignoredLiana)
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
                movementScripts.DesativateGrabandCombat();
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
        ignoredLiana = target;
        ignoreTimer = ignoreDuration;
        target = null;

        characterController.enabled = true;
        movementScripts.enabled = true;
        isStuck = false;
        movementScripts.AtivateGrabandCombat();
    }


    private void Jump()
    {
        movementScripts.JumpForward(jumpForce, 6);
    }

    private void HandleIgnoreTimer()
    {
        if (ignoredLiana != null)
        {
            ignoreTimer -= Time.deltaTime;

            if (ignoreTimer <= 0f)
            {
                ignoredLiana = null;
                ignoreTimer = 0f;
            }
        }
    }
}