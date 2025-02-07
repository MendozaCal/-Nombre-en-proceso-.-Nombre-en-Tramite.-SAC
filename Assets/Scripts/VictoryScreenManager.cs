using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bananasText;
    [SerializeField] private TextMeshProUGUI monkeysText;
    private int maxMonkeys;

    private void Start()
    {
        VerificLevel();
        int lastLevelBananas = PlayerPrefs.GetInt("LastLevelBananas", 0);
        int lastLevelMonkeys = PlayerPrefs.GetInt("LastLevelMonkeys", 0);

        bananasText.text = $"Score: {lastLevelBananas.ToString()}";
        monkeysText.text = $"Monkeys: {lastLevelMonkeys.ToString()}  / {maxMonkeys}";
    }

    public void VerificLevel()
    {
        string level = PlayerPrefs.GetString("LastLevel");
        switch (level)
        {
            case "Level 0":
                maxMonkeys = 1;
                break;
            case "Level 1":
                maxMonkeys = 3;
                break;
            case "Level 2":
                maxMonkeys = 4;
                break;
            case "Level 3":
                maxMonkeys = 4;
                break;
            case "Level 4":
                maxMonkeys = 4;
                break;
            case "Level 5":
                maxMonkeys = 5;
                break;
            default:
                maxMonkeys = 5;
                break;
        }
    }

    public void GoToLevelSelector()
    {
        SceneManager.LoadScene("LevelSelector");
    }
}