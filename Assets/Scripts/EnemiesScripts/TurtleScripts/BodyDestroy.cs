using System.Collections;
using UnityEngine;

public class BodyDestroy : MonoBehaviour
{
    public bool isTap;
    [SerializeField] GameObject Body;
    [SerializeField] GameObject Mover;
    [SerializeField] EnemyLife enemyLife;

    private Renderer objectRenderer;
    private Color originalColor;
    private void Start()
    {
        objectRenderer = Body.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }
    public void DamageInHead()
    {
        enemyLife.TakeDamage(1);
        if (objectRenderer != null && Body.activeInHierarchy)
        {
            StartCoroutine(FlashRed());
        }
    }
    private IEnumerator FlashRed()
    {
        if (objectRenderer == null) yield break;

        objectRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        objectRenderer.material.color = originalColor;
    }
}
