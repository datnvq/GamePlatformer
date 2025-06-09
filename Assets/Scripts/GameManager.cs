using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Management")]
    [SerializeField] private int currentLevelIndex;

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

    private void LoadTheEndScene() => SceneManager.LoadScene("TheEnd");

    private void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;

        SceneManager.LoadScene("Level_" + nextLevelIndex);
    }

    public void LevelFinished()
    {
        UI_FadeEffect uI_FadeEffect = UI_InGame.Instance.fadeEffect;


        bool noMoreLevel = currentLevelIndex + 2 == SceneManager.sceneCountInBuildSettings;
        if (noMoreLevel)
        {
            uI_FadeEffect.ScreenFade(1f, 1.5f, LoadTheEndScene);
        }
        else
        {
            uI_FadeEffect.ScreenFade(1f, 1.5f, LoadNextLevel);
        }

    }
}
