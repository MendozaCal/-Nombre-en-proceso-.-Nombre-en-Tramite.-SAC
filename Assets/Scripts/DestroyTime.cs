using System.Collections;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    [SerializeField] private float destroyTimer = 4f;
    [SerializeField] private string nameType = "Arma";
    [SerializeField] private GameObject bananaClusterPrefab;
    [SerializeField] private float blinkDuration = 2f;

    private bool isTouchingSomething = false;
    private bool hasSpawnedBananas = false;
    private bool isBlinking = false;

    private void Update()
    {
        GrabDestroy();
        BananasDestroy();
    }

    private void GrabDestroy()
    {
        if (!isTouchingSomething && nameType == "Arma")
        {
            destroyTimer -= Time.deltaTime;
            if (destroyTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void BananasDestroy()
    {
        if (nameType == "Bananas")
        {
            destroyTimer -= Time.deltaTime;

            if (destroyTimer <= blinkDuration && !isBlinking)
            {
                isBlinking = true;
                StartCoroutine(BlinkBeforeDestroy());
            }

            if (destroyTimer <= 0 && !hasSpawnedBananas)
            {

                Destroy(gameObject);
            }
        }
    }


    private IEnumerator BlinkBeforeDestroy()
    {
        float blinkInterval = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            bananaClusterPrefab.SetActive(!bananaClusterPrefab.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        bananaClusterPrefab.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") && nameType == "Arma")
        {
            destroyTimer = 4f;
            isTouchingSomething = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand") && nameType == "Arma")
        {
            isTouchingSomething = false;
        }
    }
}