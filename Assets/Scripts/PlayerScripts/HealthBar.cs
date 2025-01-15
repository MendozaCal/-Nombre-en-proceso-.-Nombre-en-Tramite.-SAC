using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthBarImage;
    public TextMeshProUGUI healthText;
    private float maxHealth;

    public void Initialize(float maxLife)
    {
        maxHealth = maxLife;
        UpdateHealthBar(maxHealth); 
    }

    public void UpdateHealthBar(float currentHealth)
    {
        float fillAmount = currentHealth / maxHealth;
        healthBarImage.fillAmount = fillAmount;

        healthText.text = Mathf.RoundToInt(currentHealth).ToString();  
    }
}
