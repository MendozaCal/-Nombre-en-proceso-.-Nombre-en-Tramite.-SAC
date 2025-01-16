using UnityEngine;

public class Door : MonoBehaviour
{

    public Vector3 openRotationEulerAngles; 
    public float rotationSpeed = 2f;  
    private bool isOpen = false;


    private bool hasStartedOpening = false;


    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
        targetRotation = Quaternion.Euler(openRotationEulerAngles) * initialRotation;
    }

    private void Update()
    {
        if (hasStartedOpening && !isOpen)
        {
            RotateDoor(targetRotation);
        }
    }

    private void RotateDoor(Quaternion target)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, target, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, target) < 0.1f)
        {
            isOpen = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key") && !hasStartedOpening)
        {
            hasStartedOpening = true;
        }
    }
}
