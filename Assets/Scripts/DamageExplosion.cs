using UnityEngine;

public class DamageExplosion : MonoBehaviour
{
    private bool isRecibeDamage = false;
    private PlayerLife playerLife;
    private SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        Destroy(sphereCollider, 1);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && !isRecibeDamage)
        {
            isRecibeDamage = true;
            playerLife = other.GetComponent<PlayerLife>();
            playerLife.TakeDamage(1f);
        }
    }
}
