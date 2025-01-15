using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPersecution : MonoBehaviour
{
    GameObject Turtle;
    GameObject Head;
    TortoiseMove TortoiseMove;
    TortoiseDead TortoiseDead;
    [SerializeField] int timeDestroy;
    public bool detectedPlayerArea;
    private void Start()
    {
        Turtle = GameObject.Find("Turtle");
        TortoiseMove = Turtle.GetComponent<TortoiseMove>();
        Head = GameObject.Find("Head");
        TortoiseDead = Head.GetComponent<TortoiseDead>();
    }
    private void Update()
    {
        if (TortoiseDead.isTap)
        {
            StartCoroutine(TimeToDead());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detectedPlayerArea = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detectedPlayerArea = false;
            TortoiseMove.ReturnPatroll();
        }
    }
    IEnumerator TimeToDead()
    {
        yield return new WaitForSeconds(timeDestroy);
        Destroy(gameObject);
    }
}
