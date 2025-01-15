using UnityEngine;
using System.Collections;

public interface ICombatBehavior
{
    IEnumerator ExecuteAttack(Transform target, Transform attackPoint);
}