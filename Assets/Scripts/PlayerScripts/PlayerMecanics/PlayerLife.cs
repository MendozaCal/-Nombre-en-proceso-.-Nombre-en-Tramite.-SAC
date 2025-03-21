using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : Life
{
    [SerializeField] private float initialLife = 3f;
    [SerializeField] public float pointsShield = 3f;
    [SerializeField] private float Timer = 500;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI TimerGame;
    [SerializeField] private TextMeshProUGUI BananasCont;
    [SerializeField] private GameObject KeyController;
    [SerializeField] private int bananas;
    [SerializeField] private int monkeys;
    [SerializeField] public float key;
    [SerializeField] public float totalKey;
    [SerializeField] private Image KeyImage;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Image shieldImage;
    [SerializeField] private Image MonkeyFacePosition;
    [SerializeField] private Sprite MonkeyFace1;
    [SerializeField] private Sprite MonkeyFace2;

    [Header("BoxCast Settings")]
    [SerializeField] private Vector3 boxSize = new Vector3(1f, 1f, 1f); 
    [SerializeField] private float maxDistance = 0.1f; 
    [SerializeField] private LayerMask enemyLayer;
    private bool reduceShield;
    private int multiplesProcesados = 0;
        
    [Header("References")]
    private Grab grab;
    [SerializeField] private Animator animator;

    private void Start()
    {
        grab = GetComponent<Grab>();
        PlayerPrefs.SetInt("LastLevelBananas", 0);
        PlayerPrefs.SetInt("LastLevelMonkeys", 0);
        PlayerPrefs.Save();

        int slotNumber = PlayerPrefs.GetInt("SlotNumber");
        GameSaveManager saveManager = FindObjectOfType<GameSaveManager>();
        if (saveManager != null)
        {
            SavedGame savedGame = saveManager.savedGames.Find(game => game.slotNumber == slotNumber);
            if (savedGame != null)
            {
                pointsLife = savedGame.lives;
                bananas = savedGame.collectibles;
            }
        }
        else
        {
            pointsLife = initialLife;
        }
        UnFreezePlayer();
        healthBar.Initialize(initialLife);
        healthText.text = Mathf.RoundToInt(pointsLife).ToString();
        if (totalKey >= 1)
        {
            KeyController.SetActive(true);
        }
    }
    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        Timer -= Time.deltaTime;
        TimerGame.text = Mathf.RoundToInt(Timer).ToString();
        BananasCont.text = Mathf.RoundToInt(bananas).ToString("D7");
        if (Timer <= 0)
        {
            base.TakeDamage(1);
        }
        DetectEnemies();
        UpdateShieldColor();
        ShortCuts();
    }

    private void UpdateShieldColor()
    {
        if (shieldImage != null)
        {
            if (pointsShield == 3)
            {
                shieldImage.color = Color.green;
            }
            else if (pointsShield == 2)
            {
                shieldImage.color = Color.yellow;
            }
            else if (pointsShield == 1)
            {
                shieldImage.color = Color.red;
            }
        }
        if (pointsShield < 2)
        {
            MonkeyFacePosition.sprite = MonkeyFace2;
        }
        else
        {
            MonkeyFacePosition.sprite = MonkeyFace1;
        }
    }

    public override void TakeDamage(float damage)
    {
        //base.TakeDamage(damage);
        if (pointsLife <= 0 || Time.timeScale == 0)
        {
            return;
        }

        if (!reduceShield)
        {
            pointsShield -= damage;
            if (pointsShield <= 0)
            {
                pointsShield = 3;
                ReduceLife();
                healthText.text = Mathf.RoundToInt(pointsLife).ToString();
                StartCoroutine(InvulnerabilityPeriodShield());
            }
            else
            {
                StartCoroutine(InvulnerabilityPeriodShield());
            }
            healthBar.UpdateHealthBar(pointsShield);
        }


        if (grab!= null && grab.IsHoldingObject)
        {
            grab.DropObject();
        }
    }

    private void ReduceLife()
    {
        if (pointsLife <= 0 || Time.timeScale == 0)
        {
            return;
        }

        base.TakeDamage(1);
        pointsShield = 3;
        healthBar.UpdateHealthBar(pointsShield);
        healthText.text = Mathf.RoundToInt(pointsLife).ToString();

        if (spawnPoint != null && pointsLife > 0)
        {
            StartCoroutine(DelayedRespawn()); 
        }
        else if (pointsLife <= 0)
        {
            FreezePlayer();
        }
    }

    private IEnumerator DelayedRespawn()
    {
        yield return new WaitForSeconds(0.1f); 

        CharacterController controller = GetComponent<CharacterController>();
        controller.enabled = false;
        FreezePlayer();
        Fade fade = FindObjectOfType<Fade>();
        fade.StartFadeInRespawn(spawnPoint, gameObject.transform);

        SlipperyRamp[] allRamps = FindObjectsOfType<SlipperyRamp>();
        WallClimbing[] allClimbs = FindObjectsOfType<WallClimbing>();

        foreach (SlipperyRamp ramp in allRamps)
        {
            ramp.ResetSlidingState();
        }

        foreach (WallClimbing climb in allClimbs)
        {
            climb.StopClimbing();
        }

        yield return new WaitForSeconds(1f);

        controller.enabled = true;

        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.ResetMovement();
        }

        Debug.Log("Respawn");
    }

    public void ShortCuts()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
        {
            base.Heal(1);
            healthText.text = Mathf.RoundToInt(pointsLife).ToString();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(DelayedRespawn()); 
        }
    }

    public override void Heal(float amount)
    {
        pointsShield += amount;
        if (pointsShield >= 3)
        {
            pointsShield = 3;
            bananas += 20;
        }
        healthBar.UpdateHealthBar(pointsShield);
        healthText.text = Mathf.RoundToInt(pointsLife).ToString();
    }
    private void UpdateLifeAndPoints()
    {
        int multiplesActuales = bananas / 100;
        if (multiplesActuales > multiplesProcesados)
        {
            multiplesProcesados = multiplesActuales;
            base.Heal(1);
            StartCoroutine(TextAumnetandReduce());
            pointsShield = 3;
        }
    }
    private IEnumerator TextAumnetandReduce()
    {
        float originalFontSize = healthText.fontSize; 
        float duration = 0.5f;
        float targetFontSize = 80f; 

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            healthText.fontSize = (int)Mathf.Lerp(originalFontSize, targetFontSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        healthText.fontSize = (int)targetFontSize; 
        healthText.text = Mathf.RoundToInt(pointsLife).ToString();

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            healthText.fontSize = (int)Mathf.Lerp(targetFontSize, 30f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        healthText.fontSize = 30; 
    }

    private IEnumerator InvulnerabilityPeriodShield()
    {
        reduceShield = true;
        animator.SetBool("Stun", true);
        Movement controller = GetComponent<Movement>();
        controller.stun = true;
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Stun", false); 
        yield return new WaitForSeconds(0.5f);
        controller.stun = false;
        reduceShield = false;
    }

    protected override void Die()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastLevel", currentSceneName);
        PlayerPrefs.Save();

        Fade fade = FindObjectOfType<Fade>();
        fade.StartFadeIn("GameOver");
    }

    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void DetectEnemies()
    {
        Vector3 boxCenter = transform.position;

        Vector3 direction = transform.forward;

        Quaternion orientation = transform.rotation;

        RaycastHit[] hits = Physics.BoxCastAll(
            boxCenter,          
            boxSize / 2,      
            direction,        
            orientation,        
            maxDistance,        
            enemyLayer       
        );

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                TakeDamage(1);
                break; 
            }
            if (hit.collider.CompareTag("Barrel"))
            {
                TakeDamage(1);
                Barrel barrel = hit.collider.GetComponent<Barrel>();

                if (barrel.barrelType == "Explosivo")
                {
                    barrel.InstatiatePrefabs();
                }
                else
                {
                    Destroy(hit.collider.gameObject);
                }
                break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Crocodile":
                Debug.Log("comido");
                StartCoroutine(ExecuteAnimationHazard(0.25f));
                break;

            case "Banana":
                bananas += 1;
                Destroy(other.gameObject);
                UpdateLifeAndPoints();
                break;
            case "Bananas":
                bananas += 30;
                Destroy(other.gameObject);
                UpdateLifeAndPoints();
                break;
            case "Mini":
                Heal(1);
                Destroy(other.gameObject);
                UpdateLifeAndPoints();
                break;

            case "Key":
                key++;
                KeyImage.color = Color.white;
                Destroy(other.gameObject);
                break;

            case "MonkeyColectable":
                Heal(1);
                bananas += 10;
                monkeys++;
                Destroy(other.gameObject);
                UpdateLifeAndPoints();
                break;

            case "InstantDeath":
                ReduceLife();
                break;

            case "Checkpoint":
                CheckPoint(other.transform.position);
                break;

            case "Finish":
                CompleteLevel();
                break;
            case "FinishWorld":
                ProgressWorld();
                break;
            default:
                break;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "Door":
                Debug.Log("está tocando");
                GameObject newDoor = other.gameObject;
                Door door = newDoor.gameObject.GetComponent<Door>();
                if (key >= 1 && Input.GetKey(KeyCode.E) && !door.isTouchDoor)
                {
                    key--;
                    if(key <= 0) KeyController.SetActive(false);
                    door.isTouchDoor = true;
                }
                break;
            default:
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Door":
                GameObject newDoor = other.gameObject;
                Door door = newDoor.gameObject.GetComponent<Door>();
                door.isTouchDoor = false;
                break;

            default :
                break;
        }

    }
        private IEnumerator ExecuteAnimationHazard(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ReduceLife();
    }

    private void UnFreezePlayer()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = true;
        }

        Movement movement = GetComponent<Movement>();
        movement.enabled = true;
    }

    private void FreezePlayer()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }
        
        Movement movement = GetComponent<Movement>();
        movement.enabled = false;
    }
    private void CheckPoint(Vector3 vector3)
    {
        spawnPoint.position = vector3;
    }

    public void CompleteLevel()
    {
        GameSaveManager saveManager = FindObjectOfType<GameSaveManager>();
        if (saveManager != null)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName.Contains("Level"))
            {
                int startIndex = sceneName.IndexOf("Level");
                string levelPart = sceneName.Substring(startIndex);

                string[] parts = levelPart.Split(' ');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int levelNumber))
                {
                    int slotNumber = PlayerPrefs.GetInt("SlotNumber");
                    SavedGame currentGame = saveManager.savedGames.Find(game => game.slotNumber == slotNumber);
                    if (currentGame != null)
                    {
                        if (levelNumber + 1 > currentGame.unlockedLevel)
                        {
                            currentGame.unlockedLevel = levelNumber + 1;
                        }
                        currentGame.collectibles += monkeys;
                        currentGame.lives = (int)pointsLife;
                        saveManager.SaveGames();
                        Debug.Log("Nivel desbloqueado: " + currentGame.unlockedLevel);
                    }
                }
            }
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt("LastLevelBananas", bananas);
        PlayerPrefs.SetInt("LastLevelMonkeys", monkeys);
        PlayerPrefs.SetString("LastLevel", currentSceneName);
        PlayerPrefs.Save();
        Fade fade = FindObjectOfType<Fade>();
        fade.StartFadeIn("Victory");
    }

    public void ProgressWorld()
    {
        GameSaveManager saveManager = FindObjectOfType<GameSaveManager>();
        PlayerPrefs.SetInt("LastLevelBananas", bananas);
        PlayerPrefs.SetInt("LastLevelMonkeys", monkeys);
        PlayerPrefs.SetInt("LastLevelLives", (int)pointsLife);
        PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        int slotNumber = PlayerPrefs.GetInt("SlotNumber");
        SavedGame currentGame = saveManager.savedGames.Find(game => game.slotNumber == slotNumber);
        SceneManager.LoadScene("BossLevel_" + currentGame.worldName);
    }

    public void CompleteWorldAfterBoss()
    {
        GameSaveManager saveManager = FindObjectOfType<GameSaveManager>();
        if (saveManager != null)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName.Contains("BossLevel_"))
            {
                int startIndex = sceneName.IndexOf("BossLevel_");
                string bossPart = sceneName.Substring(startIndex + "BossLevel_".Length);

                string[] parts = bossPart.Split(' ');
                if (parts.Length >= 1 && int.TryParse(parts[0], out int worldNumber))
                {
                    int slotNumber = PlayerPrefs.GetInt("SlotNumber");
                    SavedGame currentGame = saveManager.savedGames.Find(game => game.slotNumber == slotNumber);
                    if (currentGame != null)
                    {
                        currentGame.worldName = worldNumber + 1;
                        currentGame.collectibles += monkeys;
                        currentGame.lives = (int)pointsLife;
                        saveManager.SaveGames();
                        Debug.Log("Mundo desbloqueado: " + currentGame.worldName);
                    }
                }
            }
        }

        PlayerPrefs.SetInt("LastLevelBananas", bananas);
        PlayerPrefs.SetInt("LastLevelMonkeys", monkeys);
        PlayerPrefs.SetInt("LastLevelLives", (int)pointsLife);
        PlayerPrefs.Save();
        Fade fade = FindObjectOfType<Fade>();
        fade.StartFadeIn("Victory");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}