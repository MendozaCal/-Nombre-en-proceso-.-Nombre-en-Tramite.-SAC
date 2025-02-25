using UnityEngine;

public class Barrel : Life
{
    [SerializeField] private float maxLife = 1f;
    [SerializeField] private GameObject sticks;

    private void Start()
    {
        pointsLife = maxLife;
        Destroy(gameObject, 10f);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stick"))
        {
            Vector3 hitDirection = transform.position - other.transform.position;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                Vector3 horizontalDirection = Vector3.ProjectOnPlane(hitDirection, Vector3.up).normalized;
                rb.velocity = new Vector3(horizontalDirection.x * 10f, 0f, horizontalDirection.z * 10f);
                int enemyLayer = LayerMask.NameToLayer("Enemy");
                rb.excludeLayers = enemyLayer;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
            Instantiate(sticks, transform.position, Quaternion.identity);
        }  
        if(collision.gameObject.CompareTag("Gorilla"))
        {
            Destroy(gameObject);
            BossMovement bossMovement = collision.gameObject.GetComponent<BossMovement>();
            bossMovement.StartStun();
        }
    }
}