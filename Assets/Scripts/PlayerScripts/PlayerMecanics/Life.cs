using System.Collections;
using UnityEngine;

public class Life : MonoBehaviour
{
    protected float pointsLife;
    private bool receiveDamage = false;

    [SerializeField] GameObject head;
    private BodyDestroy bodyDestroy;
    public new ParticleSystem particleSystem;
    private bool isTapGrab;

    private void Start()
    {
        if (head != null)
        {
            bodyDestroy = head.GetComponent<BodyDestroy>();
        }
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }
    private void Update()
    {
        if (isTapGrab && bodyDestroy != null)
        {
            bodyDestroy.PlayerDestroy();
        }
    }
    public virtual void TakeDamage(float damage)
    {
        if (!receiveDamage) 
        {
            pointsLife -= damage;
            if (pointsLife <= 0)
            {
                pointsLife = 0;
                Die();
            }
            else
            {
                StartCoroutine(InvulnerabilityPeriod());
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
        StartCoroutine(particles());
    }
    IEnumerator particles()
    {
        isTapGrab = true;
        yield return new WaitForSeconds(10);
        Destroy(gameObject);

    }
}