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

    public void PlayerDestroy()
    {
        isTap = true;
        Body.SetActive(false);
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
