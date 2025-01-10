using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(1f, 0f, 0f);
    [SerializeField] private float pickupSpeed = 3f;
    [SerializeField] private float followSpeed = 20f;
    [SerializeField] private Vector3 holdRotation = new Vector3(0f, 0f, 0f);

    private bool isActive;
    private Transform grabbedObject;
    private bool isPickingUp;
    private float initialDistance;
    private Quaternion originalRotation; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isActive)
            {
                DropObject();
            }
            else if (grabbedObject != null)
            {
                StartGrab();
            }
        }

        if (isActive && grabbedObject != null)
        {
            Vector3 desiredPosition = target.position + target.right * offset.x + target.up * offset.y + target.forward * offset.z;

            if (isPickingUp)
            {
                grabbedObject.position = Vector3.Lerp(grabbedObject.position, desiredPosition, pickupSpeed * Time.deltaTime);

                Quaternion desiredRotation = Quaternion.Euler(holdRotation);
                grabbedObject.rotation = Quaternion.Lerp(grabbedObject.rotation, desiredRotation, pickupSpeed * Time.deltaTime);

                if (Vector3.Distance(grabbedObject.position, desiredPosition) < 0.1f)
                {
                    isPickingUp = false;
                }
            }
            else
            {
                grabbedObject.position = Vector3.Lerp(grabbedObject.position, desiredPosition, followSpeed * Time.deltaTime);
                grabbedObject.rotation = Quaternion.Euler(holdRotation);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Interactuable") && !isActive)
        {
            grabbedObject = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactuable") && !isActive)
        {
            grabbedObject = null;
        }
    }

    private void StartGrab()
    {
        isActive = true;
        isPickingUp = true;
        initialDistance = Vector3.Distance(grabbedObject.position, target.position);
        originalRotation = grabbedObject.rotation;
    }

    private void DropObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.rotation = originalRotation;
        }

        isActive = false;
        isPickingUp = false;
        grabbedObject = null;
    }
}