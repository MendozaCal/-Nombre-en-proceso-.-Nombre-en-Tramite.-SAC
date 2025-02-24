using UnityEngine;

public class EnemyLife : Life
{
    [SerializeField] private float maxLife = 1f;

    private void Start()
    {
        pointsLife = maxLife;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stick"))
        {
            TakeDamage(2);
        }
        if (other.gameObject.CompareTag("Honda"))
        {
            TakeDamage(2);
        }
    }
}