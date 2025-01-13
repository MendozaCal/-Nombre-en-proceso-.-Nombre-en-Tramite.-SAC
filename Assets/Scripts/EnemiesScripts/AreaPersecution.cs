using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPersecution : MonoBehaviour
{
    GameObject Turtle;
    TortoiseMove TortoiseMove;
    public bool detectedPlayerArea;
    private void Start()
    {
        Turtle = GameObject.Find("Turtle");
        TortoiseMove = Turtle.GetComponent<TortoiseMove>();
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
            TortoiseMove.ReturnPatroll();
        }
    }
}
