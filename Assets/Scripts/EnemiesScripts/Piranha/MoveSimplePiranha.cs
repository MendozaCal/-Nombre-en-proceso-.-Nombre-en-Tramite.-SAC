using System.Collections;
using UnityEngine;

public class MoveSimplePiranha : MonoBehaviour
{
    [Header("----Movement----")]
    public float speed;
    public float duration;
    public float timetorestart;
    private float tiempoAscenso;

    [Header("----Turn----")]
    private Rigidbody rb;
    private bool hasRotation = false;
    private bool isRotation = false;
    private Quaternion initialRotation;
    private Quaternion finalRotation;
    private float tiempoGiro;
    private float initialYRotation;
    Vector3 positionInicial;

    [Header("----Gravity----")]
    public float extraGravity = 20f; 
    private bool isFalling = false;
    private float rotationSpeedFactor = 1f;

    [Header("----Turn----")]
    AudioSource audioSource;

    void Start()
    {
        positionInicial = transform.position;
        rb = GetComponent<Rigidbody>();
        initialYRotation = transform.eulerAngles.y;
        initialRotation = transform.rotation;
        ReiniciarAscenso();
        audioSource = GetComponent<AudioSource>();
        speed = Random.Range(10f, 12f);
        duration = Random.Range(1f, 1.5f);
        timetorestart = Random.Range(0.5f, 2f);
    }

    void Update()
    {
        if (tiempoAscenso > 0)
        {
            rb.velocity = new Vector3(0, speed, 0);
            tiempoAscenso -= Time.deltaTime;
        }
        else if (!hasRotation && !isRotation) IniciarGiro();
        if (isRotation) ProgresarGiro();

        if (!isFalling && rb.velocity.y < 0)
        {
            isFalling = true;
            rb.useGravity = false; 
        }

        if (isFalling)
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration); 

            rotationSpeedFactor = Mathf.Clamp(extraGravity / 10f, 1f, 5f);  
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            StartCoroutine(Reinicio());
            isFalling = false;  
            rb.useGravity = true;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerLife playerLife = other.gameObject.GetComponent<PlayerLife>();
            playerLife.TakeDamage(1);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    IEnumerator Reinicio()
    {
        yield return new WaitForSeconds(timetorestart);
        ReiniciarAscenso();
        yield return new WaitForSeconds(0.1f);
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void ReiniciarAscenso()
    {
        transform.rotation = Quaternion.Euler(0, initialYRotation, 0);
        tiempoAscenso = duration;
        hasRotation = false;
        isRotation = false;
        tiempoGiro = 0;
        rotationSpeedFactor = 1f;  
    }

    void IniciarGiro()
    {
        initialRotation = transform.rotation;
        finalRotation = Quaternion.Euler(0, initialYRotation, transform.eulerAngles.z + 180);
        isRotation = true;
    }

    void ProgresarGiro()
    {
        tiempoGiro += Time.deltaTime * rotationSpeedFactor;  
        transform.rotation = Quaternion.Lerp(initialRotation, finalRotation, tiempoGiro);
        if (tiempoGiro >= 1)
        {
            hasRotation = true;
            isRotation = false;
        }
    }
}
