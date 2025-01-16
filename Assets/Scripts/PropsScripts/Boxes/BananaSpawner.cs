using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab; 
    [SerializeField] private int numberOfObjects = 5; 
    [SerializeField] private float spawnRadius = 1f; 
    [SerializeField] private GameObject targetObject; 

    private bool hasSpawned = false;

    private void Update()
    {
       
        if (targetObject == null && !hasSpawned)
        {
            SpawnSpheres();
            hasSpawned = true;
        }
    }

  
    private void SpawnSpheres()
    {
        Vector3 spawnCenter = transform.position; 

        for (int i = 0; i < numberOfObjects; i++)
        {
      
            Vector3 spawnPosition = spawnCenter + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius)
            );

            // Instanciar la esfera
            Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
        }

        Debug.Log("Esferas generadas tras la destrucción del objeto.");
    }
}
