using System.Collections;
using UnityEngine;

public class EnemyLife : Life
{
    [SerializeField] private float maxLife = 1f;
    private Renderer objectRenderer;
    private Color originalColor;
    [SerializeField] bool isBodyEnemy;
    [SerializeField] GameObject BodyEnemy;
    [SerializeField] int damageGrab = 2;

    private void Start()
    {
        pointsLife = maxLife;
        if (isBodyEnemy)objectRenderer = BodyEnemy.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        Debug.Log($"Vida restante de {gameObject.name}: {pointsLife}");

        if (objectRenderer != null)
        {
            StartCoroutine(FlashRed());
        }
    }
    private IEnumerator FlashRed()
    {
        objectRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        objectRenderer.material.color = originalColor;
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        if (pointsLife > maxLife)
        {
            pointsLife = maxLife;
        }
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(damageGrab);
        }
    }
}