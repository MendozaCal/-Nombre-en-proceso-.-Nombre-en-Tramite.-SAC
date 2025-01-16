using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EyesDetectorTortoise : MonoBehaviour
{
    private BoxCollider targetCollider;
    private BoxCollider currentCollider;
    public bool detectedPlayerEyes;
    /*private void Start()
    {
        GameObject targetDetectorRute = GameObject.Find("TortoiseRutePoints");
        targetCollider = targetDetectorRute.GetComponent<BoxCollider>();
        currentCollider = GetComponent<BoxCollider>();
        currentCollider.size = targetCollider.size;
        currentCollider.center = targetCollider.center;
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detectedPlayerEyes = true;
        }
    }
}
