using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] public string sceneName;
    [SerializeField] private Transform canvas;
    [SerializeField] private Fade fadePrefab;

    public void ChangeSceneWithFade()
    {
        fadePrefab.StartFadeIn(sceneName);
    }

    public void LoadSceneStart()
    {
        SceneManager.LoadScene(sceneName);
    }
}