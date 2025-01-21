using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPersecution : MonoBehaviour
{
    [SerializeField]GameObject Turtle;
    TortoiseMove TurtleMove;
    [SerializeField] GameObject RutePoints;
    public bool detectedPlayerArea;
    private void Start()
    {
        TurtleMove = Turtle.GetComponent<TortoiseMove>();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        DetectorPatrollToAttack detectorPatrollToAttack = RutePoints.GetComponent<DetectorPatrollToAttack>();
        sphereCollider.radius = detectorPatrollToAttack.distance;
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
