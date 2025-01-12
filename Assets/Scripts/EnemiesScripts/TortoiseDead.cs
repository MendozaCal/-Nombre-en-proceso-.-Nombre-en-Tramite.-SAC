using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortoiseDead : MonoBehaviour
{
    private bool isTap;
    public int timeDestroy;
    [SerializeField] GameObject Body;
    private void Update()
    {
        if (isTap)
        {
            StartCoroutine(TimeToDead());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("feet"))
        {
            isTap = true;
            Body.SetActive(false);
        }
    }
    IEnumerator TimeToDead()
    {
        yield return new WaitForSeconds(timeDestroy);
        Destroy(gameObject);
    }
}
