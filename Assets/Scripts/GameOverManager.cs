using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Button retryButton;
    [SerializeField] private Transform canvas;
    [SerializeField] private Fade fadePrefab;

    private void Start()
    {
        retryButton.onClick.AddListener(OnRetryButtonClicked);
    }

    private void OnRetryButtonClicked()
    {
        string lastLevel = PlayerPrefs.GetString("LastLevel", "Level1"); 
        fadePrefab.StartFadeIn(lastLevel);
    }
}