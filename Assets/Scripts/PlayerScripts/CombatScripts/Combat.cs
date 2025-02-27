using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    [Header("Stick")]
    [SerializeField] private float stickAttackSpeed = 10f;
    [SerializeField] private float stickReturnSpeed = 10f;

    [Header("Honda")]
    [SerializeField] private float growthSpeed = 10f;
    [SerializeField] private float maxSize = 10f;
    [SerializeField] private float cooldownTime = 2f;

    [Header("Targets")]
    [SerializeField] private Transform attackForward;  
    [SerializeField] private Transform attackUp;       
    [SerializeField] private Transform hand;          
    [SerializeField] private Transform targetPosition; 

    private Grab grabSystem;
    private Dictionary<GrabType, ICombatBehavior> combatBehaviors;
    private Vector3 handOriginalPosition;

    public static bool IsAttacking { get; private set; }

    private void Awake()
    {
        grabSystem = GetComponent<Grab>();
        InitializeCombatBehaviors();

        if (hand != null)
        {
            handOriginalPosition = hand.localPosition;
        }
    }

    private void Start()
    {
        if (targetPosition != null && hand != null)
        {
            targetPosition.position = hand.position;
        }
    }

    private void Update()
    {
        if (!IsAttacking && targetPosition != null && hand != null)
        {
            targetPosition.position = hand.position;
        }

        HandleAttack();
    }

    private void InitializeCombatBehaviors()
    {
        combatBehaviors = new Dictionary<GrabType, ICombatBehavior>
        {
            { GrabType.Stick, new StickCombat(stickAttackSpeed, stickReturnSpeed) },
            { GrabType.Honda, new HondaAttack(growthSpeed, maxSize, cooldownTime) }
        };
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

    public void ResetCombatState()
    {
        StopAllCoroutines();  
        IsAttacking = false;
        if (hand != null)
        {
            hand.localPosition = handOriginalPosition;
        }
        if (targetPosition != null && hand != null)
        {
            targetPosition.position = hand.position;
        }
    }

    private void OnEnable()
    {
        IsAttacking = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        IsAttacking = false;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetCombatState();
    }
    private IEnumerator ExecuteAttackRoutine(GrabType type)
    {
        IsAttacking = true;
        Transform launchTarget = grabSystem.GrabbedObject;

        if (launchTarget != null)
        {
            Vector3 currentTargetPosition = targetPosition.position;

            Transform attackDestination = (type == GrabType.Honda) ? attackUp : attackForward;

            yield return StartCoroutine(combatBehaviors[type].ExecuteAttack(hand, attackDestination, launchTarget));

            float returnDuration = 0.3f;
            float elapsedTime = 0;
            Vector3 startPosition = hand.position;

            while (elapsedTime < returnDuration)
            {
                float t = elapsedTime / returnDuration;
                hand.position = Vector3.Lerp(startPosition, transform.TransformPoint(handOriginalPosition), t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            hand.localPosition = handOriginalPosition;
        }
        else
        {
            Debug.LogWarning("No hay objeto agarrado para lanzar.");
        }

        IsAttacking = false;
    }
}