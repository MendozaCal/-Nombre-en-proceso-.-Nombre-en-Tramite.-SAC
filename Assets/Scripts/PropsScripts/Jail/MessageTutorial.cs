using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageTutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject jailDestruction;
    [SerializeField] bool jump;
    [SerializeField] bool liana;
    [SerializeField] bool musgo;
    DestroyEffect destroy;
    private void Start()
    {
        if (jailDestruction != null) destroy = jailDestruction.GetComponent<DestroyEffect>();
        else return;
    }
    void Update()
    {
        if (destroy.isDestroy && jailDestruction != null)
        {
            text.text = "press WASD to move";
            destroy.isDestroy = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && jump)
        {
            text.text = "press Space to jump"; StartCoroutine(Eraser());
        }
        if (other.CompareTag("Player") && liana)
        {
            text.text = "press Space to grab\nthen press Space to release"; StartCoroutine(Eraser());
        }
        if (other.CompareTag("Player") && musgo)
        {
            text.text = "press Space to jump to the moss"; StartCoroutine(Eraser());
        }
    }
    IEnumerator Eraser()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        box.enabled = false;
        yield return new WaitForSeconds(5);
        text.text = "";
        Destroy(gameObject);
    }
}
