using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance { get; private set; } 

    public List<SavedGame> savedGames = new List<SavedGame>();
    private string saveFilePath; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            return;
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "saved_games.json");
        LoadGames();
    }

    public void SaveGames()
    {
        string json = JsonUtility.ToJson(new SavedGamesWrapper { savedGames = savedGames });
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadGames()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            savedGames = JsonUtility.FromJson<SavedGamesWrapper>(json).savedGames;
        }
    }

    [System.Serializable]
    private class SavedGamesWrapper
    {
        public List<SavedGame> savedGames;
    }
}