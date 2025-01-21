using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorPatrollToAttack : MonoBehaviour
{
    public Transform objectA;
    public Transform objectB;
    public BoxCollider boxCollider;
    public float distance;
    public bool detectedPlayerRute;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (objectA != null && objectB != null && boxCollider != null)
        {
            distance = Vector3.Distance(objectA.position, objectB.position);

            boxCollider.size = new Vector3(distance, boxCollider.size.y, boxCollider.size.z);

            Vector3 midPoint = (objectA.position + objectB.position) / 2;
            boxCollider.center = transform.InverseTransformPoint(midPoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detectedPlayerRute = true;
        }
    }
}
