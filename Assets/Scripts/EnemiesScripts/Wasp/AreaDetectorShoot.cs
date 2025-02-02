using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDetectorShoot : MonoBehaviour
{
    SphereCollider Collider;
    GameObject player;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float RadiusDetector;
    [SerializeField] float spawnInterval;
    [SerializeField] Transform firePoint;
    float timer;
    bool isDetected;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Collider = GetComponent<SphereCollider>();
        Collider.radius = RadiusDetector;
    }
    void Update()
    {
        if (isDetected)Shoot();
    }
    void Shoot()
    {
        if (player != null)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                Instantiate(projectilePrefab, firePoint.transform.position, Quaternion.identity);
                timer = 0f;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isDetected = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isDetected = false;
        }
    }
}
