using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayerSelectorLevel : MonoBehaviour
{
    public Transform[] targetPoints;
    public float moveSpeed = 5f;
    private bool isMoving = false;

    public Material unlockedMaterial;
    public Material lockedMaterial;

    private GameSaveManager gameSaveManager;
    private int unlockedLevel;
    [SerializeField] private bool iskey;

    [SerializeField] private Fade fadePrefab;

    void Start()
    {
        InitializeGameData();
        if (iskey) UnlockAllLevels();
        UpdateLevelMaterials();
    }

    void Update()
    {
        HandleMovementInput();
        HandleLevelSelection();
        if (!iskey) HandleUnlockLevelInput();
    }

    private void InitializeGameData()
    {
        gameSaveManager = FindObjectOfType<GameSaveManager>();
        if (gameSaveManager != null)
        {
            int slotNumber = PlayerPrefs.GetInt("SlotNumber");
            SavedGame savedGame = gameSaveManager.savedGames.Find(game => game.slotNumber == slotNumber);
            if (savedGame != null)
            {
                unlockedLevel = savedGame.unlockedLevel;
            }
        }
    }

    private void UpdateLevelMaterials()
    {
        foreach (var point in targetPoints)
        {
            Renderer levelRenderer = point.GetComponent<Renderer>();
            LevelPoint levelPoint = point.GetComponent<LevelPoint>();

            if (levelRenderer != null && levelPoint != null)
            {
                levelRenderer.material = levelPoint.levelIndex <= unlockedLevel ? unlockedMaterial : lockedMaterial;
            }
        }
    }

    private void HandleMovementInput()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
                TryMove(Vector3.forward);
            if (Input.GetKeyDown(KeyCode.S))
                TryMove(Vector3.back);
            if (Input.GetKeyDown(KeyCode.A))
                TryMove(Vector3.left);
            if (Input.GetKeyDown(KeyCode.D))
                TryMove(Vector3.right);
        }
    }

    private void HandleLevelSelection()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectLevel();
        }
    }

    private void HandleUnlockLevelInput()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
        {
            UnlockAllLevels();
        }
    }

    private void TryMove(Vector3 direction)
    {
        Vector3 currentPos = transform.position;

        var validPoints = targetPoints
            .Where(point =>
                Vector3.Dot((point.position - currentPos).normalized, direction.normalized) > 0.9f &&
                Mathf.Approximately(point.position.y, currentPos.y) &&
                Mathf.Abs((point.position - currentPos).magnitude) > 0.1f &&
                IsLevelUnlocked(point)
            )
            .OrderBy(point => Vector3.Distance(point.position, currentPos))
            .ToList();

        if (validPoints.Count > 0)
        {
            StartCoroutine(MoveToPosition(validPoints[0].position));
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPos)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    private void SelectLevel()
    {
        Transform closestLevel = targetPoints
            .OrderBy(point => Vector3.Distance(transform.position, point.position))
            .FirstOrDefault();

        if (closestLevel != null)
        {
            LevelPoint levelPoint = closestLevel.GetComponent<LevelPoint>();
            if (levelPoint != null)
            {
                if (levelPoint.levelIndex <= unlockedLevel)
                {
                    string sceneName = "Level " + levelPoint.levelIndex;

                    if (SceneUtility.GetBuildIndexByScenePath(sceneName) >= 0)
                    {
                        Debug.Log("Cargando nivel " + levelPoint.levelIndex);
                        PlayerPrefs.SetString("SceneToLoad", sceneName);
                        fadePrefab.StartFadeIn("LoadingScreen");
                    }
                    else
                    {
                        Debug.LogError("La escena '" + sceneName + "' no está en Build Settings.");
                    }
                }
                else
                {
                    Debug.Log("Nivel " + levelPoint.levelIndex + " no está desbloqueado.");
                }
            }
        }
    }

    private void UnlockAllLevels()
    {
        unlockedLevel = targetPoints.Length;
        UpdateLevelMaterials();
        Debug.Log("Todos los niveles han sido desbloqueados.");
    }

    private bool IsLevelUnlocked(Transform levelPoint)
    {
        LevelPoint levelPointComponent = levelPoint.GetComponent<LevelPoint>();
        if (levelPointComponent != null)
        {
            return levelPointComponent.levelIndex <= unlockedLevel;
        }
        return false;
    }
}