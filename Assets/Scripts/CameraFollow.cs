using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5);
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private float smoothSpeed = 10f;

    [SerializeField] private float maxVerticalAngle = 60f;  

    private float rotationX;
    private float rotationY;
    private float initialRotationY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        rotationY = angles.y;
        rotationX = angles.x;
        initialRotationY = rotationY;
    }

    private void LateUpdate()
    {
        if (targetPlayer == null) return;

        rotationY += Input.GetAxis("Mouse X") * sensitivity;
        rotationX -= Input.GetAxis("Mouse Y") * sensitivity;

        rotationX = Mathf.Clamp(rotationX, -maxVerticalAngle, maxVerticalAngle);

        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        Vector3 desiredPosition = targetPlayer.position + rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(targetPlayer.position);
    }
}