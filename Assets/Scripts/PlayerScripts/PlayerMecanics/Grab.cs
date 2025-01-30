using UnityEngine;
public enum GrabType
{
    None,
    Stick,
    Honda,
    Key
}

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 normalOffset;
    [SerializeField] private Vector3 stickOffset;
    [SerializeField] private Vector3 hondaOffset;
    [SerializeField] private Vector3 keyOffset;

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
            if (grabbedObject != null)
            {
                StartGrab();
            }
        }
        if (isActive && Input.GetKeyDown(KeyCode.G))
        {
            DropObject();
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
            case GrabType.Key:
                target.localRotation = Quaternion.Euler(0f, 90f, 0f);
                break ;
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
            case GrabType.Key:
                return keyOffset;
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
                case "Key":
                    grabbedObject = other.transform;
                    currentGrabType = GrabType.Key;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isActive && (other.CompareTag("Stick") || other.CompareTag("Honda") || other.CompareTag("Key")))
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
        grabbedObject.SetParent(target);
    }

    public void DropObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.SetParent(null);
            grabbedObject = null;
        }
        target.localPosition = normalOffset;
        isActive = false;
        currentGrabType = GrabType.None;
    }
}