using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Transform monkey;
    [SerializeField] private float monkeyMoveDistance = 500f;
    [SerializeField] private float minLoadTime = 3f;

    private Vector3 startPos;
    private Vector3 endPos;
    private float loadStartTime;

    private void Start()
    {
        startPos = monkey.position;
        endPos = startPos + Vector3.right * monkeyMoveDistance;
        loadStartTime = Time.time;

        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float elapsedTime = Time.time - loadStartTime;
            float progress = Mathf.Clamp01(elapsedTime / minLoadTime);

            progressBar.value = progress;
            monkey.position = Vector3.Lerp(startPos, endPos, progress);

            if (elapsedTime >= minLoadTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}