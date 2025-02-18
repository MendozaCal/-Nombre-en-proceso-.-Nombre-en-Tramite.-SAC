using System.Collections;
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

    public IEnumerator Fading(string sceneName)
    {
        isFading = true;

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * speed;
            yield return null; 
        }

        if (loadScene != null)
            SceneManager.LoadScene(sceneName);
        else
            SceneManager.LoadScene("GameOver");
    }

    public void StartFadeIn(string sceneName)
    {
        if (!isFading)
            StartCoroutine(Fading(sceneName));
    }
}