using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] public LoadScene loadScene;
    [SerializeField] private float speed = 1f;
    private bool isFading = false;

    private void Start()
    {
        canvasGroup.alpha = 0f; 
    }

    private void Update()
    {
        if (!isFading) return;

        canvasGroup.alpha += Time.deltaTime * speed;

        if (canvasGroup.alpha >= 1)
        {
            if (loadScene != null) SceneManager.LoadScene(loadScene.sceneName);
            else SceneManager.LoadScene("GameOver");
        }
    }

    public void StartFadeIn()
    {
        isFading = true;
    }
}