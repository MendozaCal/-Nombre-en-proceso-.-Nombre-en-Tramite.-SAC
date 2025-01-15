using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleDamageGrab : MonoBehaviour
{
    public bool isTapGrab;
    GameObject Head;
    TortoiseDead TortoiseDead;
    private void Start()
    {
        Head = GameObject.Find("Head");
        TortoiseDead = Head.GetComponent<TortoiseDead>();
    }
    private void Update()
    {
        if (isTapGrab) TortoiseDead.PlayerDestroy();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactuable"))
        {
            isTapGrab = true;
        }
    }

}
