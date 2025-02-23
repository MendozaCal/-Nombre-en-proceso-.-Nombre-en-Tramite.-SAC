using TMPro;
using UnityEngine;

public class LoadPrefabs : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI worldNameText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI collectiblesText;

    private GameSaveManager saveManager;

    private void Start()
    {
        saveManager = FindObjectOfType<GameSaveManager>();
        LoadSlotData();
    }

    private void LoadSlotData()
    {
        if (saveManager != null)
        {
            int slotNumber = PlayerPrefs.GetInt("SlotNumber");
            SavedGame savedGame = saveManager.savedGames.Find(game => game.slotNumber == slotNumber);

            worldNameText.text = "World: " + savedGame.worldName;
            livesText.text = "x " + savedGame.lives;
            collectiblesText.text = "x " + savedGame.collectibles;

        }
    }
}
