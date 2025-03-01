using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyIfTouchPlayer : MonoBehaviour
{
    [SerializeField] float gravedadExtra = -20f; 

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
        Destroy(gameObject);
    }

}
