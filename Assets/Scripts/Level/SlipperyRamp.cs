using Unity.VisualScripting;
using UnityEngine;

public class SlipperyRamp : MonoBehaviour
{
    public float slideForce = 10f;

    public CharacterController controller;
    public Vector3 slopeDirection;
    public Vector3 lastPosition;
    private Movement movement;

    private void Start()
    {
        controller = FindAnyObjectByType<CharacterController>();
        movement = FindAnyObjectByType<Movement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (controller != null)
        {
            if (movement != null)
            {
                movement.isOnRamp = true;
                lastPosition = controller.transform.position;

                RaycastHit hit;
                if (Physics.Raycast(other.transform.position, Vector3.down, out hit, 2f))
                {
                    slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;

                    RotatePlayerTowardsSlope(slopeDirection);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ResetSlidingState();
    }

    private void FixedUpdate()
    {
        if (movement.isOnRamp && controller != null)
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

    private void RotatePlayerTowardsSlope(Vector3 slopeDir)
    {
        if (movement != null)
        {
            float targetAngle = Mathf.Atan2(slopeDir.x, slopeDir.z) * Mathf.Rad2Deg;
            movement.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
    }

    public void ResetSlidingState()
    {
        movement.isOnRamp = false;
    }
}