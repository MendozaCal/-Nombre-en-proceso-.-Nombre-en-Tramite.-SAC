using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinLogs : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;

    private void Update()
    {
        // Rotar el tiovivo sobre su eje Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
