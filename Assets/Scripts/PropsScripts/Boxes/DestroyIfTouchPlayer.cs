using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyIfTouchPlayer : MonoBehaviour
{
    [SerializeField] float gravedadExtra = -20f;
    [SerializeField] bool isFirstEvent;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.up * gravedadExtra, ForceMode.Acceleration);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Floor") && isFirstEvent)
        {
            gameObject.layer = LayerMask.NameToLayer("Floor");
            gameObject.tag = "Floor";
        }
    }
}
