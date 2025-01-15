using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float stickAttackSpeed = 10f;
    [SerializeField] private float stickReturnSpeed = 10f;
    [SerializeField] private float hondaLaunchForce = 20f;

    private Grab grabSystem;
    private Dictionary<GrabType, ICombatBehavior> combatBehaviors;
    public static bool IsAttacking { get; private set; }

    private void Awake()
    {
        grabSystem = GetComponent<Grab>();
        InitializeCombatBehaviors();
    }

    private void InitializeCombatBehaviors()
    {
        combatBehaviors = new Dictionary<GrabType, ICombatBehavior>
        {
            { GrabType.Stick, new StickCombat(stickAttackSpeed, stickReturnSpeed) },
            { GrabType.Honda, new HondaCombat(hondaLaunchForce) }
        };
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
                GrabType currentType = grabSystem.CurrentGrabType;
                if (combatBehaviors.ContainsKey(currentType))
                {
                    StartCoroutine(ExecuteAttackRoutine(currentType));
                }
            }
        }
    }

    private IEnumerator ExecuteAttackRoutine(GrabType type)
    {
        IsAttacking = true;

        Transform launchTarget = grabSystem.GrabbedObject;
        if (launchTarget != null)
        {
            yield return StartCoroutine(combatBehaviors[type].ExecuteAttack(launchTarget, attackPoint));

            grabSystem.DropObject();
        }
        else
        {
            Debug.LogWarning("No hay objeto agarrado para lanzar.");
        }

        IsAttacking = false;
    }
}