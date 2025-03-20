using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bananaPrefab; 
    [SerializeField] private GameObject shieldPrefab; 
    [SerializeField] private int numberOfBananas; 
    [SerializeField] private int numberOfShield; 
    [SerializeField] private float spawnRadius = 1f; 
    [SerializeField] private GameObject targetObject; 
    private PropsLife props;
    private bool hasSpawned = false;

    private void Start()
    {
        numberOfBananas = Random.Range(5, 10);
        numberOfShield = Random.Range(0, 3);
        props = targetObject.AddComponent<PropsLife>();
    }
    private void Update()
    {
        if (props.isDestroy == true && !hasSpawned)
        {
            spawnBananas();
            spawnShield();
            hasSpawned = true;
        }
    }  
    private void spawnBananas()
    {
        Vector3 spawnCenter = transform.position; 

        for (int i = 0; i < numberOfBananas; i++)
        {
            Vector3 spawnPosition = spawnCenter + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius)
            );
            Instantiate(bananaPrefab, spawnPosition, Quaternion.identity);
        }
    }
    private void spawnShield()
    {
        Vector3 spawnCenter = transform.position;

        for (int i = 0; i < numberOfShield; i++)
        {
            Vector3 spawnPosition = spawnCenter + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius)
            );
            Instantiate(shieldPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
