using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEvent : MonoBehaviour
{
    public List<GameObject> spawnPrefabs; 
    public float spawnInterval = 1f;
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f); 
    public float launchForce = 10f; 
    private float timeSinceLastSpawn;

    void Start()
    {
        timeSinceLastSpawn = 0f;
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnObject();
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnObject()
    {

        GameObject prefabToSpawn = spawnPrefabs[Random.Range(0, spawnPrefabs.Count)];

        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        Vector3 spawnPosition = transform.position + transform.rotation * randomPosition;

        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, transform.rotation);

        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * launchForce, ForceMode.Impulse); 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, spawnAreaSize);
        Gizmos.matrix = oldMatrix;
    }
}
