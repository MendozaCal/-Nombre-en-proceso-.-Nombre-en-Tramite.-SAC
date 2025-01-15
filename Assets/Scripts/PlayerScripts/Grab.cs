using System.Collections;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private bool useStick;
    private bool useHonda;
    private bool isActive;
    private Transform grabbedObject;

    public bool IsHoldingObject => isActive && grabbedObject != null;
    public Transform GrabbedObject => grabbedObject;
    public Transform Target => target;

    private void Update()
    {
        HandleGrab();
        if (!Combat.IsAttacking && useStick)
        {
            UpdateTargetPosition();
        }
        UpdateGrabbedObject();
    }

    private void HandleGrab()
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
    }

    private void UpdateTargetPosition()
    {
        target.localPosition = new Vector3(offset.x, offset.y, offset.z);
        target.localRotation = Quaternion.Euler(0f, 90f, 0f);
    }

    private void UpdateGrabbedObject()
    {
        if (isActive && grabbedObject != null)
        {
            grabbedObject.position = target.position;
            grabbedObject.rotation = target.rotation;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Stick") && !isActive)
        {
            grabbedObject = other.transform;
            useStick = true;
        }
        if (other.CompareTag("Honda") && !isActive)
        {
            grabbedObject = other.transform;
            useHonda = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stick") && !isActive)
        {
            grabbedObject = null;
            useStick = false;
        }
        if (other.CompareTag("Honda") && !isActive)
        {
            grabbedObject = null;
            useHonda = false;
        }
    }

    private void StartGrab()
    {
        isActive = true;
    }

    private void DropObject()
    {
        isActive = false;
        grabbedObject = null;
    }
}