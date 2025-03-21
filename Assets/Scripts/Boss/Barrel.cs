using UnityEngine;

public class Barrel : Life
{
    [SerializeField] private float maxLife = 1f;
    [SerializeField] private GameObject sticks;
    [SerializeField] public string barrelType;

    private GameObject boss;
    private Vector3 bossPosition;
    [SerializeField] private float yOffset = 1.5f;

    private void Start()
    {
        pointsLife = maxLife;
        Destroy(gameObject, 10f);

        boss = GameObject.FindGameObjectWithTag("Gorilla");
        if (boss == null)
        {
            Debug.LogWarning("No se encontró al jefe en la escena.");
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stick"))
        {
            if (barrelType == "Normal")
            {
                if (boss != null)
                {
                    bossPosition = boss.transform.position + Vector3.up * yOffset;
                }

                Vector3 directionToBoss = (bossPosition - transform.position).normalized;

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.velocity = directionToBoss * 20f;
                    int enemyLayer = LayerMask.NameToLayer("Enemy");
                    rb.excludeLayers = enemyLayer;
                }
            }
            else if (barrelType == "Explosivo")
            {
                InstatiatePrefabs();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            InstatiatePrefabs();
        }
        if (collision.gameObject.CompareTag("Gorilla"))
        {
            Destroy(gameObject);
            BossMovement bossMovement = collision.gameObject.GetComponent<BossMovement>();
            bossMovement.StartStun();
        }
    }

    public void InstatiatePrefabs()
    {
        Destroy(gameObject);
        if (sticks != null) Instantiate(sticks, transform.position, Quaternion.identity);
    }
}