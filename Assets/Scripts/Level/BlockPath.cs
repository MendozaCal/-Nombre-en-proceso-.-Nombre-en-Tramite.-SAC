using UnityEngine;

public class BlockPath : MonoBehaviour
{
    [SerializeField] private GameObject referenceObj;
    [SerializeField] private bool desactiveNow;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            referenceObj.SetActive(true);
            if (desactiveNow)
            {
                referenceObj.SetActive(false);
            }
        }
            
    }
}
