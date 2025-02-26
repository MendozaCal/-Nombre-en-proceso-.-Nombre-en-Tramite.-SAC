using System.Collections;
using UnityEngine;

public class EnemyLife : Life
{
    [SerializeField] private float maxLife = 1f;
    [SerializeField] int damageGrab = 2;
    public new ParticleSystem particleSystem;
    [SerializeField] GameObject Body;

    private void Start()
    {
        pointsLife = maxLife;
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        Debug.Log($"Vida restante de {gameObject.name}: {pointsLife}");   
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        if (pointsLife > maxLife)
        {
            pointsLife = maxLife;
        }
    }
    protected override void Die()
    {
        StartCoroutine(particles());
    }
    IEnumerator particles()
    {
        particleSystem.Play();
        Body.SetActive(false);
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stick"))
        {
            TakeDamage(damageGrab);
        }
        if (other.gameObject.CompareTag("Honda"))
        {
            TakeDamage(damageGrab);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(damageGrab);
        }
    }
}