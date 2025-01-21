using UnityEngine;

public class BodyDestroy : MonoBehaviour
{
    public bool isTap;
    [SerializeField] GameObject Body;
    public void PlayerDestroy()
    {
        isTap = true;
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
