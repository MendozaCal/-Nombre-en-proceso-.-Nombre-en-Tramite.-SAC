using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class sapo : MonoBehaviour
{
    [SerializeField] public float inflationDuration = 2f;
    [SerializeField] public float deflationDuration = 2f;
    [SerializeField] public float inflatedScale = 1.25f;
    [SerializeField] public float damagePercentage = 40f;

    [SerializeField] private float iniciodeSapo;

    private Vector3 originalScale;
    private Vector3 originalColliderSize;
    [SerializeField] public bool isInflating = false;
    [SerializeField] private int fuerzaDeEmpuje;

    [SerializeField] private Animator animator;

    [Header("AudioClip")]
    private AudioSource audioSource;

    private Collider sapoCollider;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        inflationDuration = Random.Range(0.5f, 2f);
        deflationDuration = Random.Range(0.5f, 2f);
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

        Invoke("Iniciar", iniciodeSapo);
    }
    private void Iniciar()
    {
        StartCoroutine(Inflation());
    }
    private IEnumerator Inflation()
    {
        yield return new WaitForSeconds(1f);

        isInflating = true;
        animator.SetBool("Inflando", true);
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        yield return StartCoroutine(ChangeColliderSize(originalColliderSize, originalColliderSize * inflatedScale, inflationDuration));

        yield return new WaitForSeconds(inflationDuration);

        yield return StartCoroutine(ChangeColliderSize(originalColliderSize * inflatedScale, originalColliderSize, deflationDuration));

        isInflating = false;
        animator.SetBool("Inflando", false);

        yield return new WaitForSeconds(deflationDuration);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Inflation());
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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isInflating)
        {
            Rigidbody autoRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (autoRigidbody != null)
            {
                Vector3 pushDirection = new Vector3(transform.position.x - collision.transform.position.x, 0, 0).normalized;

                autoRigidbody.AddForce(-pushDirection * fuerzaDeEmpuje, ForceMode.Impulse);

                //StartCoroutine(DisablePlayerMovement(collision.gameObject));
            }
        }
    }

    //private IEnumerator DisablePlayerMovement(GameObject player)
    //{
    //    MovementPlayer movementPlayer = player.GetComponent<MovementPlayer>();
    //    if (movementPlayer != null)
    //    {
    //        movementPlayer.enabled = false;
    //        yield return new WaitForSeconds(0.5f);
    //        movementPlayer.enabled = true;
    //    }
    //}
}
