using UnityEngine;

public class BlockPath : MonoBehaviour
{
    [SerializeField] private GameObject referenceObj;
    private void OnTriggerEnter(Collider other)
    {
        referenceObj.SetActive(true);
    }
}
