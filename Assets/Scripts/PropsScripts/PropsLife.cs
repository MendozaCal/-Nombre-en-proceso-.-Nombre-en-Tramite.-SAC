using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PropsLife : Life
{
    [SerializeField] private float maxLife = 10f;
    [SerializeField] bool isNecesary;
    [SerializeField] public bool isDestroy;
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
            StartCoroutine(TimeToDestroy(1));
        }
        else if (other.gameObject.CompareTag("Honda"))
        {
            StartCoroutine(TimeToDestroy(2));
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

    IEnumerator TimeToDestroy(int damage)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.enabled = false;
        isDestroy = true;
        yield return new WaitForSeconds(1);
        TakeDamage(damage);
    }
}