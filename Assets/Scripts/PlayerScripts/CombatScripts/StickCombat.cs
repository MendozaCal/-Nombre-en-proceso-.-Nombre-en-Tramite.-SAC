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

    public IEnumerator ExecuteAttack(Transform hand, Transform attackPoint, Transform stick)
    {
        Vector3 initialPosition = hand.position;

        CapsuleCollider stickCollider = stick.GetComponent<CapsuleCollider>();
        if (stickCollider != null)
        {
            stickCollider.excludeLayers = LayerMask.GetMask("Nothing");
        }

        float distanceThreshold = 0.1f;
        while (Vector3.Distance(hand.position, attackPoint.position) > distanceThreshold)
        {
            hand.position = Vector3.MoveTowards(hand.position, attackPoint.position, attackSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        if (stickCollider != null)
        {
            stickCollider.excludeLayers = LayerMask.GetMask("Enemy", "Prop");
        }
    }
}