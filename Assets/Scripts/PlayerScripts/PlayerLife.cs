using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : Life
{
    [SerializeField] private float maxLife = 3f;

    private void Start()
    {
        pointsLife = maxLife;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        if (pointsLife > maxLife)
        {
            pointsLife = maxLife;
        }
    }

    protected override void Die()
    {
        ReloadCurrentScene();
    }

    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentSceneName);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
}
