using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 3f;
    [SerializeField] private float resetDelay = 5f;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private Collider platformCollider;
    private Vector3 originalPosition;
    Animator animator;
    PlayerLife playerLife;
    GameObject Player;
    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        playerLife = Player.gameObject.GetComponent<PlayerLife>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("El objeto necesita un Rigidbody.");
        }
        rb.isKinematic = true;

        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("El objeto necesita un MeshRenderer.");
        }

        platformCollider = GetComponent<Collider>();
        if (platformCollider == null)
        {
            Debug.LogError("El objeto necesita un Collider.");
        }

        originalPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.enabled = true;
            Debug.Log($"Jugador ha activado la plataforma: {gameObject.name}.");
            Invoke(nameof(Fall), fallDelay);
        }
    }

    public void ActivateAnimation()
    {
        animator.enabled = true;
        Invoke(nameof(Fall), fallDelay);
    }

    public void DesactivateAnimation()
    {
        animator.enabled = false;
    }

    public void Fall()
    {
        Debug.Log($"La plataforma {gameObject.name} está cayendo.");
        rb.isKinematic = false;
        platformCollider.enabled = false;
        Invoke(nameof(DisableMesh), 2f);
        Invoke(nameof(ResetPlatform), resetDelay);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void ResetPlatform()
    {
        Debug.Log($"Restableciendo la plataforma: {gameObject.name}.");
        animator.enabled = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.isKinematic = true;
        transform.position = originalPosition; 
        meshRenderer.enabled = true; 
        platformCollider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Floor");
    }

    private void DisableMesh()
    {
        meshRenderer.enabled = false;
    }
}
