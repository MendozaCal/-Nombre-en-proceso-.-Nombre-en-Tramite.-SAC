using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TransicionCamara : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera; 
    void Start()
    {
      //  currentCamera.Priority = 1; 
    }

    
    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        currentCamera.Priority = 0;

        currentCamera = target;

        currentCamera.Priority = 1;
    }
}
