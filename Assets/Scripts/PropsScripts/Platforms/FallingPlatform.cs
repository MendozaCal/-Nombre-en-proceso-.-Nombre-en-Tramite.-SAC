using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 3f;

    [SerializeField] private Rigidbody rb;
    private bool isActivated = false;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("El objeto necesita un Rigidbody.");
        }
        rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;

            Debug.Log($"Jugador ha activado la plataforma (Trigger): {gameObject.name}.");

            Invoke(nameof(Fall), fallDelay);
        }
    }

    private void Fall()
    {
        rb.isKinematic = false; 
        Destroy(gameObject, 5f); 
    }
}
