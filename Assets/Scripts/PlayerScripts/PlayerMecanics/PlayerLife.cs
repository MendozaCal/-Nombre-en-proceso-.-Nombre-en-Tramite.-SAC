using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : Life
{
    [SerializeField] private float maxLife = 3f;
    [SerializeField] private float pointsShield = 3f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    private bool reduceShield;

    private void Start()
    {
        pointsLife = maxLife;
        healthBar.Initialize(maxLife);
        healthText.text = Mathf.RoundToInt(pointsLife).ToString(); 
    }

    public override void TakeDamage(float damage)
    {
        if (!reduceShield)
        {
            pointsShield -= damage;
            if (pointsShield <= 0)
            {
                pointsShield = 3; 
                base.TakeDamage(1);
                healthText.text = Mathf.RoundToInt(pointsLife).ToString();
                StartCoroutine(InvulnerabilityPeriodShield());
            }
            else
            {
                StartCoroutine(InvulnerabilityPeriodShield());
            }
            healthBar.UpdateHealthBar(pointsShield); 
        }
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        if (pointsLife > maxLife)
        {
            pointsLife = maxLife;
        }
        healthBar.UpdateHealthBar(pointsLife); 
        healthText.text = Mathf.RoundToInt(pointsLife).ToString(); 
    }

    private IEnumerator InvulnerabilityPeriodShield()
    {
        reduceShield = true;
        yield return new WaitForSeconds(1f);
        reduceShield = false;
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
