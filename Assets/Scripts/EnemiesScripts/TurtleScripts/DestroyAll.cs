using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAll : MonoBehaviour
{
    BodyDestroy bodyDestroy;
    [SerializeField]GameObject Head;
    [SerializeField] int timeDestroy;

    void Start()
    {
        bodyDestroy = Head.GetComponent<BodyDestroy>();
    }
    private void Update()
    {
        if (bodyDestroy != null && bodyDestroy.isTap)
        {
            StartCoroutine(TimeToDead());
        }
    }
    IEnumerator TimeToDead()
    {
        yield return new WaitForSeconds(timeDestroy);
        Destroy(gameObject);
    }
}
