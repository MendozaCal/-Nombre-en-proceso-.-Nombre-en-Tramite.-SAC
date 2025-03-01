using UnityEngine;

public class SlipperyRamp : MonoBehaviour
{
    public float slideForce = 5f;

    public CharacterController controller;
    public Vector3 slopeDirection;
    public bool isSliding = false;
    public Vector3 lastPosition;

    private void Start()
    {
        controller = FindAnyObjectByType<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (controller != null)
        {
            isSliding = true;
            lastPosition = controller.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ResetSlidingState();
    }

    private void FixedUpdate()
    {
        if (isSliding && controller != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(controller.transform.position, Vector3.down, out hit, 2f))
            {
                slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
                Debug.DrawRay(controller.transform.position, Vector3.down * 2f, Color.red, 1f); 
            }

            Vector3 moveDirection = slopeDirection * slideForce;

            if (!controller.isGrounded)
            {
                moveDirection *= 0.7f;
            }

            controller.Move(moveDirection * Time.deltaTime);
            lastPosition = controller.transform.position;
        }
    }

    public void ResetSlidingState()
    {
        isSliding = false;
    }
}