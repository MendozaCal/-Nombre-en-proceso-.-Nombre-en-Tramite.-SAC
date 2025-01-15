using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HondaAttack : MonoBehaviour
{
    public float growthSpeed = 5f;
    public float maxSize = 10f;
    public float cooldownTime = 2f;
    public Transform player;
    private SphereCollider sphereCollider;
    private bool isCooldown = false;
    private Vector3 initialPosition;
    private float initialRadius;
    private Transform grabbedObject;

    private void Start()
    {
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.center = Vector3.zero;

        initialPosition = player.position;
        initialRadius = sphereCollider.radius;
    }

    private void Update()
    {
        if (!isCooldown && Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(GrowAndReturn());
        }
    }

    private IEnumerator GrowAndReturn()
    {
        isCooldown = true;

        while (sphereCollider.radius < maxSize)
        {
            sphereCollider.radius += growthSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        while (sphereCollider.radius > initialRadius)
        {
            sphereCollider.radius -= growthSpeed * Time.deltaTime;
            yield return null;
        }

        sphereCollider.radius = initialRadius;

        sphereCollider.center = Vector3.zero;

        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Honda"))
        {
            grabbedObject = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Honda"))
        {
            grabbedObject = null;
        }
    }
}
