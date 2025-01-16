using UnityEngine;
using System.Collections;

public class HondaCombat : ICombatBehavior
{
    // codigo provicional para ver el funcionamiento separado de los grabs

    private float launchForce;

    public HondaCombat(float launchForce)
    {
        this.launchForce = launchForce;
    }

    public IEnumerator ExecuteAttack(Transform targert, Transform attackPoint, Transform honda)
    {
        Rigidbody rb = honda.GetComponent<Rigidbody>();

        if (rb != null)
        {
            honda.SetParent(null);

            Vector3 direction;

            if (attackPoint != null)
            {
                direction = (attackPoint.position - honda.position).normalized;
            }
            else
            {
                direction = honda.forward;
            }

            rb.isKinematic = false;
            rb.AddForce(direction * launchForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("El objeto lanzado no tiene un Rigidbody.");
        }

        yield return null;
    }
}
