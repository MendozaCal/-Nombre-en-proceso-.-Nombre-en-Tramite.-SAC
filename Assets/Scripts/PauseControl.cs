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

    [SerializeField] GameObject PanelPause;
    [SerializeField] GameObject PanelSettings;
    [SerializeField] GameObject PanelGamePlay;
    [SerializeField] GameObject PanelAudio;
    [SerializeField] GameObject PanelScreen;
    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string levelText = "Level";

        if (sceneName.Contains("Level"))
        {
            int startIndex = sceneName.IndexOf("Level");
            string levelPart = sceneName.Substring(startIndex);

            string[] parts = levelPart.Split(' ');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int levelNumber))
            {
                levelText = parts[0] + " " + levelNumber;
            }
        }

        textLevel.text = levelText;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!onPause) PauseGame();
            else ResumeGame();
            PanelPause.SetActive(true);
            PanelSettings.SetActive(false);
            PanelGamePlay.SetActive(false);
            PanelAudio.SetActive(false);
            PanelScreen.SetActive(false);
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
        ResumeGame();
        Fade fade = FindObjectOfType<Fade>();
        fade.StartFadeIn("MainMenu");
    }
    public void OpenSetting()
    {
        PanelSettings.SetActive(true);
        PanelPause.SetActive(false);
    }public void CloseSetting()
    {
        PanelPause.SetActive(true);
        PanelSettings.SetActive(false);
    }
    public void OpenGamePlay()
    {
        PanelGamePlay.SetActive(true);
        PanelSettings.SetActive(false);
    }public void CloseGamePlay()
    {
        PanelSettings.SetActive(true);
        PanelGamePlay.SetActive(false);
    }
    public void OpenAudio()
    {
        PanelAudio.SetActive(true);
        PanelSettings.SetActive(false);
    }public void CloseAudio()
    {
        PanelSettings.SetActive(true);
        PanelAudio.SetActive(false);
    }
    public void OpenScreen()
    {
        PanelScreen.SetActive(true);
        PanelSettings.SetActive(false);
    }public void CloseScreen()
    {
        PanelSettings.SetActive(true);
        PanelScreen.SetActive(false);
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
