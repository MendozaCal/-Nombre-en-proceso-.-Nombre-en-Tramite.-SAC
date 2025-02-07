using UnityEngine;

public class GameSelection : MonoBehaviour
{
    public GameSaveManager gameSaveManager;

    public void SelectGame(int slotNumber)
    {
        SavedGame selectedGame = gameSaveManager.savedGames.Find(game => game.slotNumber == slotNumber);
        if (selectedGame != null)
        {
            LoadGame(selectedGame);
        }
    }

    private void LoadGame(SavedGame savedGame)
    {
        PlayerPrefs.SetInt("SlotNumber", savedGame.slotNumber);
        PlayerPrefs.SetInt("UnlockedLevel", savedGame.unlockedLevel);
        PlayerPrefs.SetInt("Lives", savedGame.lives);
        PlayerPrefs.SetInt("Score", savedGame.score);
        PlayerPrefs.SetInt("Collectibles", savedGame.collectibles);

        Debug.Log("Loading game in slot: " + savedGame.slotNumber);
    }
}