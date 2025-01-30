using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : Life
{
    [SerializeField] private float initialLife = 3f;
    [SerializeField] private float pointsShield = 3f;
    [SerializeField] private float Timer = 500;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI TimerGame;
    [SerializeField] private TextMeshProUGUI BananasCont;
    [SerializeField] private TextMeshProUGUI KeyCont;
    [SerializeField] private GameObject KeyController;
    [SerializeField] private int bananas;
    [SerializeField] private int monkeys;
    [SerializeField] private float key;
    [SerializeField] private Transform spawnPoint;

    [Header("BoxCast Settings")]
    [SerializeField] private Vector3 boxSize = new Vector3(1f, 1f, 1f); 
    [SerializeField] private float maxDistance = 0.1f; 
    [SerializeField] private LayerMask enemyLayer; 

    private bool reduceShield;
    private int multiplesProcesados = 0;

    private void Start()
    {
        pointsLife = initialLife;
        healthBar.Initialize(initialLife);
        healthText.text = Mathf.RoundToInt(pointsLife).ToString();
    }
    private void Update()
    {
        Timer -= Time.deltaTime;
        TimerGame.text = Mathf.RoundToInt(Timer).ToString();
        BananasCont.text = Mathf.RoundToInt(bananas).ToString();
        KeyCont.text = Mathf.RoundToInt(key).ToString() + "/1";

        if (Timer <= 0)
        {
            base.TakeDamage(1);
        }
        DetectEnemies();
    }
    public override void TakeDamage(float damage)
    {
        if (!reduceShield)
        {
            pointsShield -= damage;
            if (pointsShield <= 0)
            {
                pointsShield = 3;
                ReduceLife();
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
        if (pointsShield >= 3)
        {
            pointsShield = 3;
        }
        healthBar.UpdateHealthBar(pointsShield);
        healthText.text = Mathf.RoundToInt(pointsLife).ToString();
    }
    private void UpdateLifeAndPoints()
    {
        int multiplesActuales = bananas / 100;
        if (multiplesActuales > multiplesProcesados)
        {
            multiplesProcesados = multiplesActuales;
            base.Heal(1);
            healthText.text = Mathf.RoundToInt(pointsLife).ToString();
            pointsShield = 3;
        }
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

    private void DetectEnemies()
    {
        Vector3 boxCenter = transform.position;

        Vector3 direction = transform.forward;

        Quaternion orientation = transform.rotation;

        RaycastHit[] hits = Physics.BoxCastAll(
            boxCenter,          
            boxSize / 2,      
            direction,        
            orientation,        
            maxDistance,        
            enemyLayer       
        );

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                TakeDamage(1);
                break; 
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Crocodile":
                StartCoroutine(ExecuteAnimationHazard(0.25f));
                break;

            case "Banana":
                Heal(1);
                bananas += 10;
                Destroy(other.gameObject);
                UpdateLifeAndPoints();
                break;

            case "Key":
                key++;
                KeyController.SetActive(true);
                Destroy(other.gameObject);
                break;

            case "MonkeyColectable":
                Heal(1);
                bananas += 20;
                monkeys++;
                Destroy(other.gameObject);
                UpdateLifeAndPoints();
                break;

            case "InstantDeath":
                ReduceLife();
                break;


            default:
                break;
        }
    }

    private IEnumerator ExecuteAnimationHazard(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ReduceLife();
    }
    private void ReduceLife()
    {
        base.TakeDamage(1);
        pointsShield = 3;
        healthBar.UpdateHealthBar(pointsShield);
        healthText.text = Mathf.RoundToInt(pointsLife).ToString();

        if (spawnPoint != null)
        {
            CharacterController controller = GetComponent<CharacterController>();
            controller.enabled = false;
            transform.position = spawnPoint.position;
            controller.enabled = true;
            Debug.Log("Respawn");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
