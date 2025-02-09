using System.Collections;
using UnityEngine;

public class StickCombat : ICombatBehavior
{
    private float attackSpeed;
    private float returnSpeed;

    public StickCombat(float attackSpeed, float returnSpeed)
    {
        this.attackSpeed = attackSpeed;
        this.returnSpeed = returnSpeed;
    }

    public IEnumerator ExecuteAttack(Transform target, Transform attackPoint, Transform stick)
    {
        CapsuleCollider boxCollider = stick.GetComponent<CapsuleCollider>();
        if (boxCollider != null)
        {
            boxCollider.excludeLayers = LayerMask.GetMask("Nothing");
        }

        while (Vector3.Distance(target.position, attackPoint.position) > 0.1f)
        {
            target.position = Vector3.MoveTowards(target.position, attackPoint.position, attackSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        Vector3 returnPosition = target.parent.position +
            target.parent.right * target.localPosition.x +
            target.parent.up * target.localPosition.y +
            target.parent.forward * target.localPosition.z;

        while (Vector3.Distance(target.position, returnPosition) > 0.1f)
        {
            target.position = Vector3.MoveTowards(target.position, returnPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        if (boxCollider != null)
        {
            boxCollider.excludeLayers = LayerMask.GetMask("Enemy", "Prop");
        }
    }
}
