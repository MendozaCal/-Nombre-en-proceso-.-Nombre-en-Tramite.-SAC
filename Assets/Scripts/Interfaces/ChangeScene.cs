using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }
    }
    public void gameManagerChange()
    {
        changeScene();
    }


    public void changeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
