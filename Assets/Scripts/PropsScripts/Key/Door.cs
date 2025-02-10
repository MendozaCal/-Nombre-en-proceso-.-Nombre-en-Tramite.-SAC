using UnityEngine;

public class Door : MonoBehaviour
{
    public float moveSpeed = 100f;
    public float newHeight = 4;
    private bool rotating;
    public bool isTouchDoor;
    private float targetHeight;

    void Update()
    {
        if (isTouchDoor && !rotating)
        {
            rotating = true;
            targetHeight = transform.position.y + newHeight;
        }

        if (rotating)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        float newY = Mathf.MoveTowards(transform.position.y, targetHeight, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        this.GetComponent<BoxCollider>().enabled = false;
        if (Mathf.Abs(transform.position.y - targetHeight) < 0.01f)
        {
            rotating = false;
            isTouchDoor = false;
        }
    }
}
