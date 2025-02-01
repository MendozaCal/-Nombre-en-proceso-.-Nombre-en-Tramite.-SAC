using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxesEvent : MonoBehaviour
{
    [SerializeField] private float destroyBoxesTime = 5f;

    private void Start()
    {
      Destroy(gameObject, destroyBoxesTime);  
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
