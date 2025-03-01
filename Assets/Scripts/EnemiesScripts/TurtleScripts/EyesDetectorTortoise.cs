using UnityEngine;

public class EyesDetectorTortoise : MonoBehaviour
{
    [SerializeField] GameObject RutePoints;
    public bool detectedPlayerEyes;

    private void Start()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        DetectorPatrollToAttack detectorPatrollToAttack = RutePoints.GetComponent<DetectorPatrollToAttack>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detectedPlayerEyes = true;
        }
    }
}
