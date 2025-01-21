using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EyesDetectorTortoise : MonoBehaviour
{
    [SerializeField] GameObject RutePoints;
    public bool detectedPlayerEyes;

    private void Start()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        DetectorPatrollToAttack detectorPatrollToAttack = RutePoints.GetComponent<DetectorPatrollToAttack>();

        float sizeZ = detectorPatrollToAttack.distance * 4;
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, sizeZ);

        float centerZ = sizeZ / 2;
        boxCollider.center = new Vector3(0, 0, centerZ);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detectedPlayerEyes = true;
        }
    }
}
