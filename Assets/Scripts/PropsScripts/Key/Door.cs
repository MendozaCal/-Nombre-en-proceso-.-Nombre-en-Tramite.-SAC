using UnityEngine;

public class Door : MonoBehaviour
{
    public float rotationSpeed = 100f; // Velocidad de rotación en grados por segundo
    private float targetAngle;
    [SerializeField] float finalAngle = 90;
    private bool rotating;
    PlayerLife PlayerLife;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerLife = player.GetComponent<PlayerLife>();
    }
    void Update()
    {
        if (PlayerLife.isTouchDoor && !rotating)
        {
            targetAngle = transform.eulerAngles.y - finalAngle;
            rotating = true;
            Debug.Log("está abiert");
        }

        if (rotating)
        {
            RotateToTarget();
        }
    }

    private void RotateToTarget()
    {
        float currentY = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentY, transform.eulerAngles.z);

        if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) < 0.01f)
        {
            rotating = false;
            PlayerLife.isTouchDoor = false;
        }
    }
}
