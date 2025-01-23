using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5);
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float positionSmoothTime = 0.1f; 

    private float rotationX;
    private float rotationY;
    private Vector3 currentVelocity; 
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        rotationY = angles.y;
        rotationX = angles.x;

        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (targetPlayer == null) return;

        rotationY += Input.GetAxis("Mouse X") * sensitivity;
        rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -maxVerticalAngle, maxVerticalAngle);

        targetRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        targetPosition = targetPlayer.position + targetRotation * offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            positionSmoothTime * Time.deltaTime
        );

        Vector3 directionToTarget = (targetPlayer.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp( 
            transform.rotation, 
            lookRotation, 
            smoothSpeed * Time.deltaTime);
    }
}