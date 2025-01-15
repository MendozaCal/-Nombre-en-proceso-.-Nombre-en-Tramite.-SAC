using System.Collections;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform target; 
    [SerializeField] private Transform attackPoint; 
    [SerializeField] private float attackSpeed = 10f; 
    [SerializeField] private float returnSpeed = 10f; 
    [SerializeField] private Vector3 offset = new Vector3(1f, 0f, 0.5f); 

    private bool isActive;
    private bool isAttacking;
    private Transform grabbedObject;

    private void Update()
    {
        HandleGrab();
        UpdateTargetPosition();
        HandleAttack();
    }

    private void HandleGrab()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isActive)
            {
                DropObject();
            }
            else if (grabbedObject != null)
            {
                StartGrab();
            }
        }
    }

    private void UpdateTargetPosition()
    {
        if (!isAttacking)
        {
            target.position = player.position + player.right * offset.x + player.up * offset.y + player.forward * offset.z;
            target.rotation = Quaternion.Euler(0f, player.eulerAngles.y + 90f, 0f);
        }

        if (isActive && grabbedObject != null)
        {
            grabbedObject.position = target.position;
            grabbedObject.rotation = target.rotation;
        }
    }

    private void HandleAttack()
    {
        if (isActive && grabbedObject != null && !isAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(MoveTargetForAttack());
            }
        }
    }

    private IEnumerator MoveTargetForAttack()
    {
        isAttacking = true;

        BoxCollider boxCollider = grabbedObject.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            LayerMask originalExcludeLayers = boxCollider.excludeLayers;

            boxCollider.excludeLayers = LayerMask.GetMask("Nothing");
        }

        while (Vector3.Distance(target.position, attackPoint.position) > 0.1f)
        {
            target.position = Vector3.MoveTowards(target.position, attackPoint.position, attackSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f); 

        while (Vector3.Distance(target.position, player.position + player.right * offset.x + player.up * offset.y + player.forward * offset.z) > 0.1f)
        {
            target.position = Vector3.MoveTowards(target.position, player.position + player.right * offset.x + player.up * offset.y + player.forward * offset.z, returnSpeed * Time.deltaTime);
            yield return null;
        }

        target.position = player.position + player.right * offset.x + player.up * offset.y + player.forward * offset.z;
        target.rotation = player.rotation;

        if (boxCollider != null)
        {
            LayerMask originalExcludeLayers = boxCollider.excludeLayers;

            boxCollider.excludeLayers = LayerMask.GetMask("Enemy");
        }
        isAttacking = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Stick") && !isActive)
        {
            grabbedObject = other.transform;
        }
        if (other.CompareTag("Honda") && !isActive)
        {
            grabbedObject = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stick") && !isActive)
        {
            grabbedObject = null;
        }
        if (other.CompareTag("Honda") && !isActive)
        {
            grabbedObject = null;
        }
    }

    private void StartGrab()
    {
        isActive = true;
    }

    private void DropObject()
    {
        isActive = false;
        grabbedObject = null;
    }
}