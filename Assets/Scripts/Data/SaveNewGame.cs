using UnityEngine;

public class SaveNewGame : MonoBehaviour
{
    public GameSaveManager gameSaveManager;

    public void SaveGame(int slotNumber, string worldName, int lives, int score, int collectibles)
    {
        SavedGame newGame = new SavedGame
        {
            slotNumber = slotNumber,
            worldName = worldName,
            lives = lives,
            score = score,
            collectibles = collectibles,
            unlockedLevel = 0 
        };

        gameSaveManager.savedGames.Add(newGame);
        gameSaveManager.SaveGames();
    }
}
