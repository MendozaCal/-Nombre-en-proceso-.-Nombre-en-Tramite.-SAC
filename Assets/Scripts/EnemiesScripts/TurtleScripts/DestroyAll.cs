using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAll : MonoBehaviour
{
    [SerializeField] GameObject Head;
    [SerializeField] int timeDestroy;

    private void Update()
    {
        if (Head == null)
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
