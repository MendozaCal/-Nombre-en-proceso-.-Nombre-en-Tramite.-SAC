using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    private float destroyTimer = 4f; 
    private bool isTouchingSomething = false; 

    private void Update()
    {
        if (!isTouchingSomething)
        {
            destroyTimer -= Time.deltaTime;

            if (destroyTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            destroyTimer = 4f;
            isTouchingSomething = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            isTouchingSomething = false;
        }
    }
}