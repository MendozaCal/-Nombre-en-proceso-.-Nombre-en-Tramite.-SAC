using UnityEngine;

public class SaveNewGame : MonoBehaviour
{
    public GameSaveManager gameSaveManager;

    public void SaveGame(int slotNumber, string worldName, int lives, int collectibles)
    {
        SavedGame newGame = new SavedGame
        {
            slotNumber = slotNumber,
            worldName = worldName,
            lives = lives,
            collectibles = collectibles
        };

        gameSaveManager.savedGames.Add(newGame);
        gameSaveManager.SaveGames();
    }
}
