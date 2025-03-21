using UnityEngine;

public class MovingPiranha : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float pauseDuration = 1f;
    [SerializeField] private Transform fishModel;
    [SerializeField] private float fishTurnSpeed = 180f;

    private float currentRotation = 0f; 
    [SerializeField] private int rotationDirection = 1; 
    private bool isPaused = false;
    private float pauseTimer = 0f; 
    private bool isTurningFish = false;
    [SerializeField] private Quaternion initialRotation = Quaternion.Euler(0f, 90f, 0f);
    [SerializeField] private Quaternion finalRotation = Quaternion.Euler(180f, -90f, 0f);
    private bool isFacingInitial = true;

    void Start()
    {
        if (fishModel != null)
        {
            fishModel.rotation = initialRotation;
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

        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    void TriggerFishTurn()
    {
        isPaused = true; 
        isTurningFish = true;
        isFacingInitial = !isFacingInitial; 
    }

    void RotateFishModel()
    {
        Quaternion targetRotation = isFacingInitial ? initialRotation : finalRotation;

        fishModel.rotation = Quaternion.RotateTowards(fishModel.rotation, targetRotation, fishTurnSpeed * Time.deltaTime);

        if (Quaternion.Angle(fishModel.rotation, targetRotation) < 0.1f)
        {
            isTurningFish = false; 
        }
    }
}
