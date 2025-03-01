using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEvent : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPrefabs;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
    [SerializeField] private float launchForce;
    [SerializeField] private float timeSinceLastSpawn;
    [SerializeField] private float destroyPropsTime = 5f;
    [SerializeField] bool ifEvent;
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
        //if (ifEvent) 
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
            launchForce = Random.Range(75f, 100f);

            rb.AddForce(transform.forward * launchForce, ForceMode.Impulse); 
        }

        Destroy(spawnedObject, destroyPropsTime);

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
