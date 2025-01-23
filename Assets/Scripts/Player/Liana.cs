using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liana : MonoBehaviour
{
    public float speed = 1.0f;
    private float maxAngle = 45.0f;
    [SerializeField] private bool MoveLiana;
    [SerializeField] private BoxCollider LianaBox;
    private float LianaActive;
    void Update()
    {
        if (MoveLiana) 
        {
            float angle = maxAngle * Mathf.Sin(Time.time * speed);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
