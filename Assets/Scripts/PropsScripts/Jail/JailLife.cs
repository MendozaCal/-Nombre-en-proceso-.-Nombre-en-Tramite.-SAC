using UnityEngine;

public class JailLife : Life
{
    [SerializeField] private float maxLife = 10f;
    private Transform parentObject;
    private void Start()
    {
        pointsLife = maxLife;
        
    }
    private void Awake()
    {        
        parentObject = transform.parent;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stick"))
        {
            TakeDamage(1);
        }
        else if (other.gameObject.CompareTag("Honda"))
        {
            TakeDamage(2);
        }
    }
}
