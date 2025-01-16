using UnityEngine;
public enum GrabType
{
    None,
    Stick,
    Honda
}

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 stickOffset;
    [SerializeField] private Vector3 hondaOffset;

    private bool isActive;
    private Transform grabbedObject;
    private GrabType currentGrabType = GrabType.None;

    public bool IsHoldingObject => isActive && grabbedObject != null;
    public Transform GrabbedObject => grabbedObject;
    public Transform Target => target;
    public GrabType CurrentGrabType => currentGrabType;

    private void Update()
    {
        HandleGrab();
        if (!Combat.IsAttacking && IsHoldingObject)
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
        Vector3 offset = GetOffsetForCurrentType();

        target.localPosition = offset;

        switch (currentGrabType)
        {
            case GrabType.Stick:
                target.localRotation = Quaternion.Euler(0f, 90f, 0f); 
                break;
            case GrabType.Honda:
                target.localRotation = Quaternion.identity;
                break;
            default:
                target.localRotation = Quaternion.identity;
                break;
        }

        if (grabbedObject != null)
        {
            grabbedObject.position = target.position;
            grabbedObject.rotation = target.rotation;
        }
    }


    private Vector3 GetOffsetForCurrentType()
    {
        switch (currentGrabType)
        {
            case GrabType.Stick:
                return stickOffset;
            case GrabType.Honda:
                return hondaOffset;
            default:
                return Vector3.zero;
        }
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
        if (!isActive)
        {
            switch (other.tag)
            {
                case "Stick":
                    grabbedObject = other.transform;
                    currentGrabType = GrabType.Stick;
                    break;
                case "Honda":
                    grabbedObject = other.transform;
                    currentGrabType = GrabType.Honda;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isActive && (other.CompareTag("Stick") || other.CompareTag("Honda")))
        {
            if (other.transform == grabbedObject)
            {
                grabbedObject = null;
                currentGrabType = GrabType.None;
            }
        }
    }

    private void StartGrab()
    {
        isActive = true;
    }

    public void DropObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.SetParent(null);
            grabbedObject = null;
        }
        isActive = false;
        currentGrabType = GrabType.None;
    }
}