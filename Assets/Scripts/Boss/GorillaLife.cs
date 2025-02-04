using UnityEngine;

public class GorillaLife : Life
{
    [SerializeField] private float maxLife = 3f;

    private void Start()
    {
        pointsLife = maxLife;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
}