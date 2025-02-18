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

        if (PointA == null || PointB == null)
        {
            Debug.LogError("Faltan referencias a PointA o PointB en el Inspector.");
            return;
        }

        // Calcular el centro entre PointA y PointB
        Vector3 centerPosition = (PointA.position + PointB.position) / 2f;

        // Centrar el collider en el punto medio (convertido a coordenadas locales)
        sphereCollider.center = transform.InverseTransformPoint(centerPosition);

        // Ajustar el radio del collider en base a la distancia entre los puntos
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
