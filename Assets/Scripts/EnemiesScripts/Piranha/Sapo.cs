using System.Collections;
using UnityEngine;

public class sapo : MonoBehaviour
{
    [SerializeField] bool NotRandom;
    [SerializeField] public float inflationDuration = 2f;
    [SerializeField] public float deflationDuration = 2f;
    [SerializeField] public float inflatedScale = 1.25f;
    [SerializeField] public float damagePercentage = 40f;

    [SerializeField] private float iniciodeSapo;
    [SerializeField] public bool damage;

    private Vector3 originalScale;
    private Vector3 originalColliderSize;
    [SerializeField] public bool isInflating = false;
    [SerializeField] private int fuerzaDeEmpuje;

    [SerializeField] private Animator animator;

    [Header("AudioClip")]
    private AudioSource audioSource;

    private Collider sapoCollider;
        
    [SerializeField] private bool playerDetected = false;
    [SerializeField] public float timeSinceLastDetection = 0f;
    private float timeToDeflate = 2f; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!NotRandom)
        {
            inflationDuration = Random.Range(0.5f, 2f);
            deflationDuration = Random.Range(0.5f, 2f);
        }
    }

    private void Awake()
    {
        sapoCollider = GetComponent<Collider>();
        originalScale = transform.localScale;

        if (sapoCollider is BoxCollider boxCollider)
        {
            originalColliderSize = boxCollider.size;
        }
        else if (sapoCollider is SphereCollider sphereCollider)
        {
            originalColliderSize = Vector3.one * sphereCollider.radius;
        }
        else if (sapoCollider is CapsuleCollider capsuleCollider)
        {
            originalColliderSize = new Vector3(capsuleCollider.radius * 2, capsuleCollider.height, capsuleCollider.radius * 2);
        }
    }

    private void Update()
    {
        if (playerDetected)
        {
            timeSinceLastDetection = 0f;
        }
        else
        {
            timeSinceLastDetection += Time.deltaTime;

            if (timeSinceLastDetection >= timeToDeflate && isInflating)
            {
                StartCoroutine(Deflate());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            if (!isInflating)
            {
                StartCoroutine(Inflation());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }

    private IEnumerator Inflation()
    {
        animator.SetBool("Inflando", true);
        isInflating = true;

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        yield return new WaitForSeconds(0.25f);

        yield return StartCoroutine(ChangeColliderSize(originalColliderSize, originalColliderSize * inflatedScale, inflationDuration));

        while (playerDetected)
        {
            yield return null; 
        }
    }

    private IEnumerator Deflate()
    {
        yield return StartCoroutine(ChangeColliderSize(originalColliderSize * inflatedScale, originalColliderSize, deflationDuration));

        isInflating = false;
        animator.SetBool("Inflando", false);
    }

    private IEnumerator ChangeColliderSize(Vector3 startSize, Vector3 endSize, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 currentSize = Vector3.Lerp(startSize, endSize, elapsedTime / duration);
            if (sapoCollider is BoxCollider boxCollider)
            {
                boxCollider.size = currentSize;
            }
            else if (sapoCollider is SphereCollider sphereCollider)
            {
                sphereCollider.radius = currentSize.x / 2f;
            }
            else if (sapoCollider is CapsuleCollider capsuleCollider)
            {
                capsuleCollider.radius = currentSize.x / 2f;
                capsuleCollider.height = currentSize.y;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (sapoCollider is BoxCollider finalBoxCollider)
        {
            finalBoxCollider.size = endSize;
        }
        else if (sapoCollider is SphereCollider finalSphereCollider)
        {
            finalSphereCollider.radius = endSize.x / 2f;
        }
        else if (sapoCollider is CapsuleCollider finalCapsuleCollider)
        {
            finalCapsuleCollider.radius = endSize.x / 2f;
            finalCapsuleCollider.height = endSize.y;
        }
    }
}