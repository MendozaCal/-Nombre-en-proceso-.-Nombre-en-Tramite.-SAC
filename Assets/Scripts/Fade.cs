using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] public LoadScene loadScene;
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool changeScene;
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

        if (changeScene)
            SceneManager.LoadScene(sceneName);
        else
            SceneManager.LoadScene("GameOver");
    }

    public IEnumerator Fading(Transform targert, Transform player)
    {
        isFading = true;

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * speed;
            yield return null;
        }
            player.transform.position = targert.position;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * speed;
            yield return null;
        }
        isFading = false;
    }

    public void StartFadeIn(string sceneName)
    {
        if (!isFading)
            StartCoroutine(Fading(sceneName));
    }
    public void StartFadeInRespawn(Transform targert, Transform player)
    {
        if (!isFading)
            StartCoroutine(Fading(targert, player));
    }
}