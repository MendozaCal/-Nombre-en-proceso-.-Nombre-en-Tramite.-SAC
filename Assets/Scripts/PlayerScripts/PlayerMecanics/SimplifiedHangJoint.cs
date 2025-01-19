using UnityEngine;

public class SimplifiedHangJoint : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Rigidbody lianaRigidbody;
    [SerializeField] private Movement movemetScript;

    [Header("Configuración de Movimiento")]
    [SerializeField] private float swingForce = 15f; 
    [SerializeField] private float detachForce = 10f;

    [Header("Configuración de Colgar/Descolgar")]
    [SerializeField] private float detachCooldown = 0.5f;

    private bool isStuck = false;
    private bool isNearLiana = false;
    private GameObject currentLiana;
    private Transform anchorPoint;
    private LianaDesignor designor;
    private float lastDetachTime;

    private void Update()
    {
        HandleInput();

        if (isStuck)
        {
            HandleSwingMovement();
            SyncPlayerWithLiana();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isStuck)
            {
                Detach();
            }
            else if (isNearLiana)
            {
                Attach();
            }
        }
    }

    private void Attach()
    {
        if (currentLiana == null) return;

        movemetScript.enabled = false;
        isStuck = true;
        lianaRigidbody = currentLiana.GetComponent<Rigidbody>();
        anchorPoint = currentLiana.transform;

        if (anchorPoint != null)
        {
            characterController.enabled = false;
            designor = currentLiana.GetComponent<LianaDesignor>();
            anchorPoint = designor.ObjectPosition.transform; 
            transform.position = anchorPoint.position;
        }
    }

    private void Detach()
    {
        if (Time.time - lastDetachTime < detachCooldown) return;

        isStuck = false;
        characterController.enabled = true;

        lastDetachTime = Time.time;
        movemetScript.enabled = true;
        movemetScript.Jump(detachForce);
    }


    private void HandleSwingMovement()
    {
        if (lianaRigidbody == null || anchorPoint == null) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = (playerCamera.forward * verticalInput + playerCamera.right * horizontalInput).normalized;
        inputDirection.y = 0;

        Vector3 swingForceDirection = inputDirection * swingForce;
        lianaRigidbody.AddForce(swingForceDirection, ForceMode.Acceleration);


        float maxSwingSpeed = 10f; 
        if (lianaRigidbody.velocity.magnitude > maxSwingSpeed)
        {
            lianaRigidbody.velocity = lianaRigidbody.velocity.normalized * maxSwingSpeed;
        }
    }


    private void SyncPlayerWithLiana()
    {
        if (anchorPoint == null) return;

        transform.position = anchorPoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Liana"))
        {
            isNearLiana = true;
            currentLiana = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentLiana)
        {
            isNearLiana = false;
            currentLiana = null;
        }
    }
}