using UnityEngine;

public class GorillaLife : Life
{
    [SerializeField] private float maxLife = 3f;
    [SerializeField] private PlayerLife playerLife;

    private void Start()
    {
        pointsLife = maxLife;
    }
    private void Update()
    {
        Dead();
    }

    public void Dead()
    {
        if (pointsLife <= 0)
        {
            Invoke("CallCompleteWorldAfterBoss", 3f);
        }
    }

    private void CallCompleteWorldAfterBoss()
    {
        if (playerLife != null)
        {
            playerLife.CompleteWorldAfterBoss();
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
}