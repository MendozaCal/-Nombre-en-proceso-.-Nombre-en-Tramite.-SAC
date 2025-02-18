using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinLogs : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float rotationMaxSpeed = 100;
    [SerializeField] private float rotationMinSpeed = 50;
    [SerializeField] bool rotacionContraria;
    private void Start()
    {
        rotationSpeed = Random.Range(rotationMinSpeed, rotationMaxSpeed);
        if (rotacionContraria) rotationSpeed *= -1;
    }
    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
