using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelSelection : MonoBehaviour
{
    [SerializeField] private UI_LevelButton buttonPrefab;
    [SerializeField] private Transform buttonParent;

    private void Start()
    {
        CreateLevelButtons();
    }

    private void CreateLevelButtons()
    {
        int levelAmount = SceneManager.sceneCountInBuildSettings - 1;

        for (int i = 1; i < levelAmount; i++)
        {
            UI_LevelButton button = Instantiate(buttonPrefab, buttonParent);
            button.SetupButtons(i); // Assuming levels are 1-indexed
        }
    }
}
