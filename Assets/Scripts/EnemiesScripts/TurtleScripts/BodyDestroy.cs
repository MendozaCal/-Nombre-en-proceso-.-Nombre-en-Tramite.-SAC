using UnityEngine;

public class BodyDestroy : MonoBehaviour
{
    public bool isTap;
    [SerializeField] GameObject Body;
    ParticleSystem ParticleSystem;
    Vector3 vector3;
    private void Start()
    {
        ParticleSystem = Body.GetComponent<ParticleSystem>();
        ParticleSystem.Stop();
    }
    public void PlayerDestroy()
    {
        isTap = true;
        ParticleSystem.Play();
        Body.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("feet"))
        {
            PlayerDestroy();
        }
    }
}
