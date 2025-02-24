using UnityEngine;

public class BodyDestroy : MonoBehaviour
{
    public bool isTap;
    [SerializeField] GameObject Body;
    [SerializeField] GameObject Mover;
    [SerializeField] EnemyLife enemyLife;
    ReceiveDamage ReceiveDamage;
    private void Start()
    {
        ReceiveDamage = Mover.GetComponent<ReceiveDamage>();
    }
    public void PlayerDestroy()
    {
        isTap = true;
        Body.SetActive(false);
        ReceiveDamage.particleSystem.Play();
    }
    public void DamageInHead()
    {
        enemyLife.TakeDamage(1);
    }
}
