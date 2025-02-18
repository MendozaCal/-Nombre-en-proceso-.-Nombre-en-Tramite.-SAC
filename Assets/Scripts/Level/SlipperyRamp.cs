using UnityEngine;

public class SlipperyRamp : MonoBehaviour
{
    public float slideForce = 5f;
    public float repositionThreshold = 5f; 

    private CharacterController controller;
    private Vector3 slopeDirection;
    private bool isSliding = false;
    private Vector3 lastPosition; 

    private void OnTriggerStay(Collider other)
    {
        controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(other.transform.position, Vector3.down, out hit, 2f))
            {
                slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
            }

            isSliding = true;
            lastPosition = controller.transform.position; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() == controller)
        {
            isSliding = false;
            controller = null;
        }
    }

    private void FixedUpdate()
    {
        if (isSliding && controller != null)
        {
            if (Vector3.Distance(controller.transform.position, lastPosition) > repositionThreshold)
            {
                ResetSlidingState();
                return;
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
        controller = null;
    }
}