using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelSelection : MonoBehaviour
{
    [SerializeField] private UI_LevelButton buttonPrefab;
    [SerializeField] private Transform buttonParent;

    [SerializeField] private bool[] levelUnlocked; // Array to track unlocked levels

    private void Start()
    {
        LoadLevelInfo();
        CreateLevelButtons();
    }

    private void CreateLevelButtons()
    {
        int levelAmount = SceneManager.sceneCountInBuildSettings - 1;

        for (int i = 1; i < levelAmount; i++)
        {
            if (!IsLevelUnlocked(i)) return; // Skip locked levels

            UI_LevelButton button = Instantiate(buttonPrefab, buttonParent);
            button.SetupButtons(i); // Assuming levels are 1-indexed
        }
    }

    private bool IsLevelUnlocked(int levelIndex) => levelUnlocked[levelIndex];

    private void LoadLevelInfo()
    {
        int levelAmount = SceneManager.sceneCountInBuildSettings - 1;

        levelUnlocked = new bool[levelAmount];

        for(int i = 1; i < levelAmount; i++)
        {
            bool isUnlocked = PlayerPrefs.GetInt("Level_" + i + "_Unlocked", 0) == 1;

            if (isUnlocked)
            {
                levelUnlocked[i] = true; // Store unlocked status
            }
        }

        levelUnlocked[1] = true; // First level is always unlocked


    }
}
