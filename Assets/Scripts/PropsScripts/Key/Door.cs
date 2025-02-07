using UnityEngine;

public class Door : MonoBehaviour
{
    private PlayerLife playerLife;

    public Vector3 openRotationEulerAngles; 
    public float rotationSpeed = 2f;  
    private bool isOpen = false;


    public bool hasStartedOpening = false;


    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        playerLife = FindObjectOfType<PlayerLife>();
        initialRotation = transform.rotation;
        targetRotation = Quaternion.Euler(openRotationEulerAngles) * initialRotation;
    }

    private void Update()
    {
        if (hasStartedOpening  && Input.GetKeyDown(KeyCode.E))
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player") && !hasStartedOpening && (playerLife.key >= 1))
    //    {
    //        Debug.Log("Toco puerta");
    //        hasStartedOpening = true;
    //    }
    //}

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Toco puerta");
            hasStartedOpening = true;
        }

    }
}
