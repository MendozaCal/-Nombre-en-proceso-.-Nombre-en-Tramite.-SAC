using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovePos : MonoBehaviour
{
   
    [SerializeField] private float rotationDuration = 5f;    
    [SerializeField] private float openAngle = 180f;
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private float pauseDuration = 5f;

    private Quaternion endRotation;
    private Quaternion startRotation;
    private bool isMoving = true;
    private float rotationTimer = 0f;
    private float delayTimer = 0f; 
    private bool isWaiting = true;

    private void Start()
    {
        endRotation = transform.localRotation;
        startRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + openAngle);
        delayTimer = startDelay;
    }

    private void Update()
    {
        if (isWaiting)
        {

            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0f)
            {
                isWaiting = false;
                rotationTimer = 0f;
            }
            return;
        }


        rotationTimer += Time.deltaTime;


        float t = Mathf.Clamp01(rotationTimer / rotationDuration);


        transform.localRotation = isMoving
            ? Quaternion.Lerp(endRotation, startRotation, t)
            : Quaternion.Lerp(startRotation, endRotation, t);

        if (t >= 1f)
        {
            isMoving = !isMoving ;
            isWaiting = true;
            delayTimer = pauseDuration;
        }
    }
}

