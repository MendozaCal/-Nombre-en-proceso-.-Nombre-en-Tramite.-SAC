using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combat : MonoBehaviour
{
    [Header("Stick")]
    [SerializeField] private float stickAttackSpeed = 10f;
    [SerializeField] private float stickReturnSpeed = 10f;

    [Header("Honda")]
    [SerializeField] private float growthSpeed = 10f;
    [SerializeField] private float maxSize = 10f;
    [SerializeField] private float cooldownTime = 2f;

    [SerializeField] private Transform attackFoward;
    [SerializeField] private Transform attackUp;
    [SerializeField] private Transform target;



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
            { GrabType.Honda, new HondaAttack(growthSpeed, maxSize , cooldownTime) }
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
            if (type == GrabType.Honda) // esto me parece que deberia ser otro switch
            {
                yield return StartCoroutine(combatBehaviors[type].ExecuteAttack(launchTarget, attackUp));
            }
            else
            {
                yield return StartCoroutine(combatBehaviors[type].ExecuteAttack(target, attackFoward));
            }
        }
        else
        {
            Debug.LogWarning("No hay objeto agarrado para lanzar.");
        }

        IsAttacking = false;
    }
}