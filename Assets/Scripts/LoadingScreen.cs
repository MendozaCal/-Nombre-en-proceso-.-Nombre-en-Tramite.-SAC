using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private Transform monkey;  
    [SerializeField] private float monkeyMoveDistance = 500f;
    [SerializeField] private float minLoadTime = 3f; 

    private Vector3 startPos; 
    private Vector3 endPos;  
    private float loadStartTime;

    private void Start()
    {
        string sceneToLoad = PlayerPrefs.GetString("SceneToLoad", "MainMenu");

        startPos = monkey.position;
        endPos = startPos + Vector3.right * monkeyMoveDistance;
        loadStartTime = Time.time;

        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; 

        while (!operation.isDone)
        {

            float elapsedTime = Time.time - loadStartTime;
            float timeProgress = Mathf.Clamp01(elapsedTime / minLoadTime); 
            float loadProgress = Mathf.Clamp01(operation.progress / 0.9f); 

            float totalProgress = Mathf.Min(timeProgress, loadProgress);

            progressBar.value = totalProgress;
            monkey.position = Vector3.Lerp(startPos, endPos, totalProgress);

            if (elapsedTime >= minLoadTime && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true; 
            }

            yield return null; 
        }
    }
}