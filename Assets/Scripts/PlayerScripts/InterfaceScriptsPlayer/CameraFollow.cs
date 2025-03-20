using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject targetPlayer;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5);
    [SerializeField] private Vector3 climbingOffset = new Vector3(0, 3, -3);
    [SerializeField] private float maxsensitivity = 5f;
    private float sensitivity;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float positionSmoothTime = 0.1f;
    [SerializeField] private float rotationSmoothTime = 0.1f;
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private float minDistance = 0.5f;
    Movement movement;
    [Header("UI Elements")]
    [SerializeField] private Slider sensitivitySlider;

    private float rotationX;
    private float rotationY;
    private Vector3 currentVelocity;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Quaternion currentRotation;

    private void Start()
    {
        movement = targetPlayer.GetComponent<Movement>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        rotationY = angles.y;
        rotationX = angles.x;
        targetPosition = transform.position;
        targetRotation = transform.rotation;
        currentRotation = transform.rotation;

        sensitivity = maxsensitivity / 2;

        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = 1f;
            sensitivitySlider.maxValue = maxsensitivity;
            sensitivitySlider.value = sensitivity;
            sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        }
    }

    private void LateUpdate()
    {
        if (targetPlayer == null) return;

        if (!movement.isInClimbing)
        {
            rotationY += Input.GetAxis("Mouse X") * sensitivity;
            rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
            rotationX = Mathf.Clamp(rotationX, -maxVerticalAngle, maxVerticalAngle);
            targetRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
        else
        {
            targetRotation = movement.rotationWall;
        }

        if (movement.isInClimbing)
        {
            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSmoothTime * Time.deltaTime);
        }
        else if (!movement.isInClimbing && currentRotation != targetRotation)
        {
            currentRotation = targetRotation;
        }

        Vector3 currentOffset = movement.isInClimbing ? climbingOffset : offset;

        Vector3 desiredPosition = targetPlayer.transform.position + currentRotation * currentOffset;

        Vector3 directionToCamera = (desiredPosition - targetPlayer.transform.position).normalized;
        float targetDistance = currentOffset.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(targetPlayer.transform.position, directionToCamera, out hit, targetDistance, collisionLayers))
        {
            float distanceToHit = Mathf.Max(hit.distance - 0.1f, minDistance);
            targetPosition = targetPlayer.transform.position + directionToCamera * distanceToHit;
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

        transform.rotation = currentRotation;
    }

    public void UpdateSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }

}