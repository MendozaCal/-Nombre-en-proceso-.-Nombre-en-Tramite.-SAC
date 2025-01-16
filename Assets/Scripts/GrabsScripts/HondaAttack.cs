using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HondaAttack : ICombatBehavior
{
    private float growthSpeed;
    private float maxSize;
    private float cooldownTime;

    public HondaAttack(float growthSpeed, float maxSize, float cooldownTime)
    {
        this.growthSpeed = growthSpeed;
        this.maxSize = maxSize;
        this.cooldownTime = cooldownTime;
    }

    public IEnumerator ExecuteAttack(Transform target, Transform attackPoint)
    {
        SphereCollider sphereCollider = attackPoint.GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            sphereCollider = attackPoint.gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
        }

        float initialRadius = sphereCollider.radius;

        // Fase de crecimiento
        while (sphereCollider.radius < maxSize)
        {
            sphereCollider.radius += growthSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Fase de regreso
        while (sphereCollider.radius > initialRadius)
        {
            sphereCollider.radius -= growthSpeed * Time.deltaTime;
            yield return null;
        }

        sphereCollider.radius = initialRadius;

        // Enfriamiento
        yield return new WaitForSeconds(cooldownTime);
    }
}

