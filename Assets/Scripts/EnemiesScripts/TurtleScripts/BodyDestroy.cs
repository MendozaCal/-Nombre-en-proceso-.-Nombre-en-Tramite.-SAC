using UnityEngine;

public class BodyDestroy : MonoBehaviour
{
    public bool isTap;
    [SerializeField] GameObject Body;
    [SerializeField] GameObject Mover;
    ReceiveDamage ReceiveDamage;
    private void Start()
    {
        ReceiveDamage = Mover.GetComponent<ReceiveDamage>();
    }
    public void PlayerDestroy()
    {
        isTap = true;
        Body.SetActive(false);
        ReceiveDamage.particleSystem.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("feet"))
        {
            PlayerDestroy();
        }
    }
}
