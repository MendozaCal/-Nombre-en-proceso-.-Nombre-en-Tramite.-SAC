using System.Collections;
using UnityEngine;

public class Life : MonoBehaviour
{
    protected float pointsLife;
    private bool receiveDamage = false;

    public virtual void TakeDamage(float damage)
    {
        if (!receiveDamage) 
        {
            StartCoroutine(InvulnerabilityPeriod());
            pointsLife -= damage;
            if (pointsLife <= 0)
            {
                Die();
            }
        }
    }

    private IEnumerator InvulnerabilityPeriod()
    {
        receiveDamage = true;
        yield return new WaitForSeconds(1f);
        receiveDamage = false;
    }

    public virtual void Heal(float amount)
    {
        pointsLife += amount;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}