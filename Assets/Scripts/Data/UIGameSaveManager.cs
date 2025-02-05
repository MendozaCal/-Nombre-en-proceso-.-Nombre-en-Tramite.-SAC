using UnityEngine;
using UnityEngine.UI;

public class UIGameSaveManager : MonoBehaviour
{
    public GameSaveManager gameSaveManager; 
    public GameObject saveSlotPrefab; 
    public Transform saveSlotsContainer; 

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (Transform child in saveSlotsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var savedGame in gameSaveManager.savedGames)
        {
            GameObject saveSlotUI = Instantiate(saveSlotPrefab, saveSlotsContainer);
            saveSlotUI.GetComponent<SaveSlotUI>().Setup(savedGame);
        }
    }
}

public class SaveSlotUI : MonoBehaviour
{
    public Text slotNumberText;
    public Text worldNameText;
    public Text livesText;
    public Text collectiblesText;

    public void Setup(SavedGame savedGame)
    {
        slotNumberText.text = "Slot: " + savedGame.slotNumber;
        worldNameText.text = "World: " + savedGame.worldName;
        livesText.text = "Lives: " + savedGame.lives;
        collectiblesText.text = "Collectibles: " + savedGame.collectibles;
    }
}