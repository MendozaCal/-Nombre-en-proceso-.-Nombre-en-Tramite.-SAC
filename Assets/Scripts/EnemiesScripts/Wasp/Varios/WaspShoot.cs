using System.Collections;
using UnityEngine;
using UnityEngine.WSA;

public class WaspShoot : MonoBehaviour
{
    [Header("----Detector----")]
    [SerializeField] GameObject Wasp;
    public GameObject player;
    public bool isDetected;

    [Header("----Shoot----")]
    public GameObject projectilePrefab;
    public GameObject firePoint;
    public float spawnInterval = 2f;
    private float timer = 0f;
    public GameObject Detector;

    [Header("----References----")]
    public WaspDetector detector;
    public AvistaPatrol avistaPatrol;
    public EnemyLife enemyLife;
    private bool isShoot;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        detector = Detector.GetComponent<WaspDetector>();
        enemyLife = GetComponent<EnemyLife>();
        avistaPatrol = GetComponent<AvistaPatrol>();
    }
    void Update()
    {
        if(enemyLife.isDead) return; 
        if (player == null) player = GameObject.FindWithTag("Player");
        if (Wasp != null && detector.PlayerStay)
        {
            Wasp.transform.LookAt(player.transform.position);
            Shoot();
        }
    }
    void Shoot()
    {
        if (player != null)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval && !isShoot)
            {
                isShoot = true;
                Instantiate(projectilePrefab, firePoint.transform.position, Quaternion.identity);
                StartCoroutine(StopMovementForSeconds(1f));
            }
        }
    }

    IEnumerator StopMovementForSeconds(float seconds)
    {
        avistaPatrol.enabled = false;
        yield return new WaitForSeconds(seconds);
        avistaPatrol.enabled = true;
        isShoot = false;
        timer = 0f;
    }
}
