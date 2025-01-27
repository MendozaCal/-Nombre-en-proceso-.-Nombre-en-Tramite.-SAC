using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liana : MonoBehaviour
{
    public float speed = 1.0f;
    private float maxAngle = 45.0f;
    [SerializeField] private bool MoveLiana = true;
    [SerializeField] private BoxCollider LianaBox;
    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (MoveLiana)
        {
            float angle = maxAngle * Mathf.Sin(Time.time * speed);

            transform.rotation = startRotation * Quaternion.Euler(0, 0, angle);
        }
    }
}
