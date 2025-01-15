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

    public IEnumerator ExecuteAttack(Transform grabbedObject, Transform attackPoint)
    {
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            grabbedObject.SetParent(null);

            Vector3 direction = (attackPoint.position - grabbedObject.position).normalized;

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
