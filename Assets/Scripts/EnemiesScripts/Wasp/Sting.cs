using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sting : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 10;
    GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = direction * projectileSpeed;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
