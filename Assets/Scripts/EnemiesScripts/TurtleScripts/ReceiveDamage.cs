using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDamage : MonoBehaviour
{
    public bool isTapGrab;
    public List<string> targetTags = new List<string>();
    [SerializeField] GameObject head;
    private BodyDestroy bodyDestroy;
    public new ParticleSystem particleSystem;
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
        if (head != null)
        {
            bodyDestroy = head.GetComponent<BodyDestroy>();
        }
        AddTag("Stick");
        AddTag("Honda");
    }

    public void AddTag(string tag)
    {
        if (!targetTags.Contains(tag))
        {
            targetTags.Add(tag);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetTags.Contains(other.gameObject.tag))
        {
            isTapGrab = true;
        }
    }

}
