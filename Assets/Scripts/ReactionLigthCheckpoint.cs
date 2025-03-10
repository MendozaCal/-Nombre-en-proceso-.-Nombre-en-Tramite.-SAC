using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionLigthCheckpoint : MonoBehaviour
{
    [SerializeField] Light light1;
    [SerializeField] Light light2;
    bool isEnter;
    [SerializeField] float cycleTime = 2f;

    void Update()
    {
        if (isEnter)
        {
            float pingPongValue = Mathf.PingPong(Time.time / cycleTime, 1);

            float intensity = Mathf.Lerp(1, 8, pingPongValue);
            float range = Mathf.Lerp(2, 3, pingPongValue);

            light1.intensity = intensity;
            light2.intensity = intensity;

            light1.range = range;
            light2.range = range;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) isEnter = true;
    }
}
