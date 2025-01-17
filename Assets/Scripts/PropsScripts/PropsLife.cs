using UnityEngine;

public class PropsLife : Life
{
    [SerializeField] private float maxLife = 10f; 

    private void Start()
    {
        pointsLife = maxLife; 
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage); 

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
    }


    public virtual void DestroyProp()
    {
        Debug.Log($"{gameObject.name} fue destruido.");
        
            Destroy(gameObject);        
    }
}