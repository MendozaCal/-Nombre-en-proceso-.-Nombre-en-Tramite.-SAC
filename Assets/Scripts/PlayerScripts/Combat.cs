using System.Collections;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackSpeed = 10f;
    [SerializeField] private float returnSpeed = 10f;

    private Grab grabSystem;
    public static bool IsAttacking { get; private set; }

    private void Awake()
    {
        grabSystem = GetComponent<Grab>();
    }

    private void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (grabSystem.IsHoldingObject && !IsAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(MoveTargetForAttack());
            }
        }
    }

    private IEnumerator MoveTargetForAttack()
    {
        IsAttacking = true;
        Transform target = grabSystem.Target;
        BoxCollider boxCollider = grabSystem.GrabbedObject.GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            LayerMask originalExcludeLayers = boxCollider.excludeLayers;
            boxCollider.excludeLayers = LayerMask.GetMask("Nothing");
        }

        Vector3 startPosition = target.position;
        while (Vector3.Distance(target.position, attackPoint.position) > 0.1f)
        {
            target.position = Vector3.MoveTowards(target.position, attackPoint.position, attackSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        Vector3 returnPosition = target.parent.position + target.parent.right * target.localPosition.x + target.parent.up * target.localPosition.y + target.parent.forward * target.localPosition.z;

        while (Vector3.Distance(target.position, returnPosition) > 0.1f)
        {
            target.position = Vector3.MoveTowards(target.position, returnPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        if (boxCollider != null)
        {
            LayerMask originalExcludeLayers = boxCollider.excludeLayers;
            boxCollider.excludeLayers = LayerMask.GetMask("Enemy");
        }

        IsAttacking = false;
    }
}