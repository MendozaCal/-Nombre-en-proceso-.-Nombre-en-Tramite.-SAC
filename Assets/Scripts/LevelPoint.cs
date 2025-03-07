using UnityEngine;

public class LevelPoint : MonoBehaviour
{
    public int levelIndex;
    [SerializeField] GameObject LevelName;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSelector"))
        {
            LevelName.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSelector"))
        {
            LevelName.SetActive(false);
        }
    }
}