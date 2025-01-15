using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : Life
{
    [SerializeField] private float maxLife = 3f;
    [SerializeField] private HealthBar healthBar; 

    private void Start()
    {
        pointsLife = maxLife;
        healthBar.Initialize(maxLife); 
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateHealthBar(pointsLife); 
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        if (pointsLife > maxLife)
        {
            pointsLife = maxLife;
        }
        healthBar.UpdateHealthBar(pointsLife); 
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
