using UnityEngine;

public class PropsLife : Life
{
    [SerializeField] private float maxLife = 10f;
    [SerializeField] bool isNecesary;
    [SerializeField] bool isWoodWall;

    private void Start()
    {
        pointsLife = maxLife;
        if (isNecesary)
        {
            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            particleSystem.Play();
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        Debug.Log($"Vida restante de {gameObject.name}: {pointsLife}");

        if (pointsLife <= 0)
        {
            DestroyProp();
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
        if (isWoodWall && other.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
        }
    }

    public virtual void DestroyProp()
    {
        Debug.Log($"{gameObject.name} fue destruido.");
        
            Destroy(gameObject);        
    }
}