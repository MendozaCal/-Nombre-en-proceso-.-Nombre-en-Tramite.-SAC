using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PartidaSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI slotNumberText;
    public TextMeshProUGUI worldNameText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI collectiblesText;

    [Header("Panels")]
    public GameObject newGamePanel;
    public GameObject loadGamePanel;
    public GameObject deletePanel;

    [Header("Buttons in Panels")]
    public Button createAndPlayButton;
    public Button loadAndPlayButton;
    public Button deleteButton;

    private int slotNumber;
    private GameSaveManager saveManager;
    private static PartidaSlotUI selectedSlot;
    private bool hasExistingSave;

    private void Start()
    {
        saveManager = FindObjectOfType<GameSaveManager>();
        slotNumber = transform.GetSiblingIndex() + 1;
        slotNumberText.text = "Slot " + slotNumber;

        GetComponent<Button>().onClick.AddListener(OnSlotClick);

        if (slotNumber == 1)
        {
            ConfigurePanelsAndButtons();
        }

        LoadSlotData();
    }

    private void ConfigurePanelsAndButtons()
    {
        if (newGamePanel == null || loadGamePanel == null)
        {
            Debug.LogError("Panels not assigned! Please assign them in the inspector.");
            return;
        }

        createAndPlayButton.onClick.AddListener(() => {
            if (selectedSlot != null)
                selectedSlot.CreateNewGame();
        });

        loadAndPlayButton.onClick.AddListener(() => {
            if (selectedSlot != null && selectedSlot.hasExistingSave)
                selectedSlot.SelectGame();
        });

        HidePanels();
    }

    private void OnSlotClick()
    {
        if (selectedSlot == this)
        {
            selectedSlot = null;
            HidePanels();
            return;
        }

        selectedSlot = this;
        UpdatePanelsVisibility();
    }

    private void LoadSlotData()
    {
        if (saveManager != null)
        {
            SavedGame savedGame = saveManager.savedGames.Find(game => game.slotNumber == slotNumber);
            hasExistingSave = (savedGame != null);

            if (hasExistingSave)
            {
                worldNameText.text = "World: " + savedGame.worldName;
                livesText.text = "Lives: " + savedGame.lives;
                collectiblesText.text = "Collectibles: " + savedGame.collectibles;
                deleteButton.gameObject.SetActive(hasExistingSave);
            }
            else
            {
                worldNameText.text = "Empty";
                livesText.text = "";
                collectiblesText.text = "";
            }
        }
    }

    private void UpdatePanelsVisibility()
    {
        HidePanels();

        if (hasExistingSave)
        {
            loadGamePanel.SetActive(true);
            newGamePanel.SetActive(false);
        }
        else
        {
            newGamePanel.SetActive(true);
            loadGamePanel.SetActive(false);
        }
    }

    private void HidePanels()
    {
        if (newGamePanel != null) newGamePanel.SetActive(false);
        if (loadGamePanel != null) loadGamePanel.SetActive(false);
        if (deletePanel != null) deletePanel.SetActive(false);
    }

    public void ActivatePanelDelete()
    {
        if (deletePanel != null) deletePanel.SetActive(true);
    }

    public void DeleteGame()
    {
        if (!hasExistingSave) return;

        SavedGame gameToDelete = saveManager.savedGames.Find(game => game.slotNumber == slotNumber);
        if (gameToDelete != null)
        {
            saveManager.savedGames.Remove(gameToDelete);
            saveManager.SaveGames();
        }

        hasExistingSave = false;
        LoadSlotData();

        deleteButton.gameObject.SetActive(hasExistingSave);
        HidePanels();
    }


    public void SelectGame()
    {
        if (!hasExistingSave) return; 

        PlayerPrefs.SetInt("SlotNumber", slotNumber);
        SceneManager.LoadScene("LevelSelector");
    }

    public void CreateNewGame()
    {
        PlayerPrefs.SetInt("SlotNumber", slotNumber);
        SavedGame newGame = new SavedGame
        {
            slotNumber = slotNumber,
            worldName = "Level 0",
            lives = 3,
            collectibles = 0
        };
        saveManager.savedGames.Add(newGame);
        saveManager.SaveGames();
        SceneManager.LoadScene("LevelSelector");
    }
}