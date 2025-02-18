using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPersecution : MonoBehaviour
{
    [SerializeField] private GameObject Turtle;
    private TortoiseMove TurtleMove;
    [SerializeField] private Transform PointA;
    [SerializeField] private Transform PointB;
    private SphereCollider sphereCollider;
    public bool detectedPlayerArea;

    private void Start()
    {
        TurtleMove = Turtle.GetComponent<TortoiseMove>();
        sphereCollider = GetComponent<SphereCollider>();
        Vector3 centerPosition = (PointA.position + PointB.position) / 2f;
        sphereCollider.center = transform.InverseTransformPoint(centerPosition);
        sphereCollider.radius = Vector3.Distance(PointA.position, PointB.position) / 2f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detectedPlayerArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detectedPlayerArea = false;
            TurtleMove.ReturnPatroll();
        }
    }
}
