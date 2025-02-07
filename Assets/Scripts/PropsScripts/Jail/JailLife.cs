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
        Debug.Log($"Vida restante de {gameObject.name}: {pointsLife}");

        if (pointsLife <= 0)
        {
            DestroyJailProp();
        }
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

    public virtual void DestroyJailProp()
    {

        if (parentObject != null)
        {
            Debug.Log($"{parentObject.name} y sus hijos fueron destruidos.");
            Destroy(parentObject.gameObject); 
        }
    }
}
