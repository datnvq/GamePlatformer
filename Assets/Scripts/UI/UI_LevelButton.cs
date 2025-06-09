using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNameText;
    private string levelName;
    private int levelIndex;

    public void SetupButtons(int newLevelIndex)
    {
        levelIndex = newLevelIndex;

        levelName = "Level_" + levelIndex;
        levelNameText.text = "Level " + levelIndex;
    }

    public void LoadLevel()
    {
        if (!string.IsNullOrEmpty(levelName))
        {
           SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogWarning("Level name is not set!");
        }
    }
}
