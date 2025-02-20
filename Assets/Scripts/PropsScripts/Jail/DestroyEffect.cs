using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    [SerializeField] GameObject jailLife;
    [SerializeField] float forzeEfect;
    Rigidbody rb;
    [SerializeField] bool isTecho;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (jailLife == null)
        {
            rb.isKinematic = false;
            StartCoroutine(DestroyElement());
            if (isTecho) transform.position += Vector3.right * 10 * Time.deltaTime;
            else
            {
                Quaternion targetRotation = Quaternion.Euler(forzeEfect, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }
    IEnumerator DestroyElement()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
