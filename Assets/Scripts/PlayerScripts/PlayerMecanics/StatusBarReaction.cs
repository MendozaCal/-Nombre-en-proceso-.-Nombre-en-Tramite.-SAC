using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarReaction : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    void Update()
    {
        transform.LookAt(Camera.transform.position);
    }
}
