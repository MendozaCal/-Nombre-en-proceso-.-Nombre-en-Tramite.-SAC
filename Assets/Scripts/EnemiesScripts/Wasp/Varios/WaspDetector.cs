using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspDetector : MonoBehaviour
{
    public bool PlayerStay;
    float triggerCooldown = 0.5f;
    float lastTriggerTime = 0;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Time.time >= lastTriggerTime + triggerCooldown)
        {
            PlayerStay = true;
            lastTriggerTime = Time.time;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStay = false;
        }
    }
}
