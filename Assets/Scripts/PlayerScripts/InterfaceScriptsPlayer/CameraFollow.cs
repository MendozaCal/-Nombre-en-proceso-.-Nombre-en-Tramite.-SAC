using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5);
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float positionSmoothTime = 0.1f;
    [SerializeField] private LayerMask collisionLayers; 
    [SerializeField] private float minDistance = 0.5f;

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

        Vector3 desiredPosition = targetPlayer.position + targetRotation * offset;

        Vector3 directionToCamera = (desiredPosition - targetPlayer.position).normalized;
        float targetDistance = offset.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(targetPlayer.position, directionToCamera, out hit, targetDistance, collisionLayers))
        {
            float distanceToHit = Mathf.Max(hit.distance - 0.1f, minDistance);
            targetPosition = targetPlayer.position + directionToCamera * distanceToHit;
        }
        else
        {
            targetPosition = desiredPosition;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            positionSmoothTime * Time.deltaTime
        );

        transform.rotation = targetRotation;
    }
}