using TMPro;
using UnityEngine;

public class GorillaLife : Life
{
    [SerializeField] private float maxLife = 3f;
    [SerializeField] private PlayerLife playerLife;
    [SerializeField] private GameObject[] imageGorilaLife;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private void Start()
    {
        pointsLife = maxLife;
        UpdateGorilaLifeImages();
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
        UpdateGorilaLifeImages();
    }
    private void UpdateGorilaLifeImages()
    {
        int imagesToActivate = Mathf.CeilToInt(pointsLife);

        for (int i = 0; i < imageGorilaLife.Length; i++)
        {
            if (i < imagesToActivate)
            {
                imageGorilaLife[i].SetActive(true);
            }
            else
            {
                imageGorilaLife[i].SetActive(false);
            }
        }
    }
}