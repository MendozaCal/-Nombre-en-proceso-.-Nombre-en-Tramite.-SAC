using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PauseControl : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private GameObject panel;
    [SerializeField] Volume postProcessingVolume;
    [SerializeField] TextMeshProUGUI textLevel;
    [SerializeField] AudioMixer audioMixer;
    private bool onPause;

    private void Start()
    {
        textLevel.text = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!onPause) PauseGame();
            else ResumeGame();
        }
    }

    public void PauseGame()
    {
        onPause = true;
        cameraFollow.enabled = false;
        postProcessingVolume.enabled = true;
        MuteSounds();
        panel.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        onPause = false;
        cameraFollow.enabled = true;
        postProcessingVolume.enabled = false;
        UnmuteSounds();
        panel.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResetGame()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void MuteSounds()
    {
        audioMixer.SetFloat("MasterVolume", -80f); 
    }

    public void UnmuteSounds()
    {
        audioMixer.SetFloat("MasterVolume", 0f); 
    }
}
