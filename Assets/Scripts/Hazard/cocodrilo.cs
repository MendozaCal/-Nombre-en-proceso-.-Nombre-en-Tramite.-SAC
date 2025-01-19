using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cocodrilo : MonoBehaviour
{
    private float TiempoaEsperar;
    public bool PuedeMorder;
    private bool condicionCumplida;
    public GameObject objeto;
    [SerializeField] private Animator animator;
    [Header("AudioClip")]
    AudioSource audioSource;
    void Start()
    {
        objeto.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (PuedeMorder == true)
        {
            TiempoaEsperar = 0;
        }
        else
        {
            TiempoaEsperar += Time.deltaTime;
            condicionCumplida = false;
        }
        if (TiempoaEsperar > 2.5f)
        {
            PuedeMorder = true;
        }
    }
    void OnTriggerStay(Collider collision)
    {
        if (PuedeMorder && collision.CompareTag("Player"))
        {
            if (!condicionCumplida)
            {
                Debug.Log("Mordio");
                condicionCumplida = true;
                StartCoroutine(Esperar());
            }
        }
    }
    IEnumerator Esperar()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Mordio", true);
        objeto.SetActive(true);
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Mordio", false);
        objeto.SetActive(false);
        PuedeMorder = false;
    }
}
