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
    [SerializeField] private float bananas;
    [SerializeField] private float monkeys;

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
        pointsShield += amount;
        if (pointsShield >= 3 && pointsLife <= 2)
        {
            pointsShield = 1;
            base.Heal(amount);
        }
        if (pointsShield >= 3 && pointsLife >= maxLife)
        {
            pointsShield = 3;
            pointsLife = maxLife;
        }
        healthBar.UpdateHealthBar(pointsShield);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crocodile"))
        {
            StartCoroutine(ExecuteAnimationHazard(0.25f));
        }
        if (other.CompareTag("Banana"))
        {
            Heal(1);
            bananas++;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("MonkeyColectable"))
        {
            Heal(1);
            bananas++;
            monkeys++;
            Destroy(other.gameObject);
        }
    }
    private IEnumerator ExecuteAnimationHazard(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        base.TakeDamage(1);
        healthText.text = Mathf.RoundToInt(pointsLife).ToString();
    }
}
