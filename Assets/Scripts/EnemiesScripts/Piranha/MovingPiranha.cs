using UnityEngine;

public class MovingPiranha : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float pauseDuration = 1f;
    [SerializeField] private Transform fishModel;
    [SerializeField] private float fishTurnSpeed = 180f;

    private float currentRotation = 0f;
    private int rotationDirection = 1;
    private bool isPaused = false;
    private float pauseTimer = 0f;
    private bool isTurningFish = false;

    [SerializeField] private Vector3 initialModelRotation = new Vector3(0f, 90f, 0f);
    [SerializeField] private Vector3 finalModelRotation = new Vector3(180f, -90f, 0f);
    private bool isFacingInitial = true;

    private Quaternion effectiveInitialRotation;
    private Quaternion effectiveFinalRotation;
    private Quaternion objectInitialRotation;

    void Start()
    {
        objectInitialRotation = transform.rotation;

        effectiveInitialRotation = objectInitialRotation * Quaternion.Euler(initialModelRotation);
        effectiveFinalRotation = objectInitialRotation * Quaternion.Euler(finalModelRotation);

        if (fishModel != null)
        {
            fishModel.rotation = effectiveInitialRotation;
        }
    }

    void Update()
    {
        if (isTurningFish)
        {
            RotateFishModel();
            return;
        }

        if (isPaused)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseDuration)
            {
                isPaused = false;
                pauseTimer = 0f;
            }
            return;
        }

        float rotationStep = rotationSpeed * Time.deltaTime * rotationDirection;
        currentRotation += rotationStep;

        if (currentRotation >= 180)
        {
            currentRotation = 180;
            rotationDirection = -1;
            TriggerFishTurn();
        }
        else if (currentRotation <= 0)
        {
            currentRotation = 0;
            rotationDirection = 1;
            TriggerFishTurn();
        }

        transform.rotation = objectInitialRotation * Quaternion.Euler(0f, 0f, currentRotation);
    }

    void TriggerFishTurn()
    {
        isPaused = true;
        isTurningFish = true;
        isFacingInitial = !isFacingInitial;
    }

    void RotateFishModel()
    {
        Quaternion targetRotation = isFacingInitial ? effectiveInitialRotation : effectiveFinalRotation;
        fishModel.rotation = Quaternion.RotateTowards(fishModel.rotation, targetRotation, fishTurnSpeed * Time.deltaTime);

        if (Quaternion.Angle(fishModel.rotation, targetRotation) < 0.1f)
        {
            isTurningFish = false;
        }
    }
}