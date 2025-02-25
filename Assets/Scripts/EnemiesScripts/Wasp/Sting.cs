using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sting : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float lifeTime = 5f;

    private Rigidbody rb;
    private Vector3 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position + new Vector3(0, 1.5f, 0);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * projectileSpeed;
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stick") || other.gameObject.CompareTag("Honda"))
        {
            Vector3 returnDirection = (initialPosition - transform.position).normalized;
            rb.velocity = returnDirection * projectileSpeed;
        }
    }
}
