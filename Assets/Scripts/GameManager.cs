using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Management")]
    [SerializeField] private int currentLevelIndex;
    private int nextLevelIndex;

    [Header("Player Management")]
    public Player player;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    public float respawnDelay = 1f;

    [Header("Fruit Management")]
    [SerializeField] private int fruitCollected;
    [SerializeField] private bool fruitAreRandom;
    [SerializeField] private int totalFruit;

    [Header("Check Point")]
    [SerializeField] private bool _canBeReactive;


    [Header("Traps")]
    public GameObject ArrawPrefab;



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
        }
        player = FindAnyObjectByType<Player>();
    }

    private void Start()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        nextLevelIndex = currentLevelIndex + 1;
        CollectFruitInfo();
    }

    private void CollectFruitInfo()
    {
        Fruit[] allFruit = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruit = allFruit.Length;
    }

    public bool CanBeReactive() => _canBeReactive;

    public void RespawnPlayer() => StartCoroutine(RespawnPlayerCoroutine());

    public void UpdateRespawnPlayer(Transform newRespawnPoint) => playerSpawnPoint = newRespawnPoint;

    private IEnumerator RespawnPlayerCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        GameObject newPLayer = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        player = newPLayer.GetComponent<Player>();
    }

    public void SpawnArraw(Vector2 position, float reSpawnTime)
    {
        StartCoroutine(SpawnArrawCoroutine(position, reSpawnTime));
    }

    private IEnumerator SpawnArrawCoroutine(Vector2 position, float reSpawnTime)
    {
        yield return new WaitForSeconds(reSpawnTime);
        GameObject newArraw = Instantiate(ArrawPrefab, position, Quaternion.identity);
    }

    public void AddFruit() => ++fruitCollected;
    public bool HaveRandomLookFruit() => !fruitAreRandom;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentLevelIndex = scene.buildIndex;
        nextLevelIndex = currentLevelIndex + 1;
        CollectFruitInfo();
    }

    public void LevelFinished()
    {
        SaveProgression();
        LoadNextScene();

    }

    private void SaveProgression()
    {
        PlayerPrefs.SetInt("Level_" + nextLevelIndex + "Unlocked", 1);

        if (NoMoreLevel() == false)
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
    }

    private void LoadTheEndScene() => SceneManager.LoadScene("TheEnd");

    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level_" + nextLevelIndex);
    }


    private void LoadNextScene()
    {
        UI_FadeEffect uI_FadeEffect = UI_InGame.Instance.fadeEffect;


        
        if (NoMoreLevel())
        {
            uI_FadeEffect.ScreenFade(1f, 1.5f, LoadTheEndScene);
        }
        else
        {
            uI_FadeEffect.ScreenFade(1f, 1.5f, LoadNextLevel);
        }
    }

    private bool NoMoreLevel() => currentLevelIndex + 2 == SceneManager.sceneCountInBuildSettings;
}
