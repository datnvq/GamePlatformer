using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Credits : MonoBehaviour
{
    private UI_FadeEffect fadeEffect;
    [SerializeField] private RectTransform creditsPanel;
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private float offScreenPositionY = 1600f;

    private bool skipCredits = false;

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
        fadeEffect.ScreenFade(0f, 1f); // Fade in to transparent over 1.5 seconds
    }

    private void Update()
    {
        if (creditsPanel != null)
        {
            // Move the credits panel upwards
            creditsPanel.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }

        if(creditsPanel != null && creditsPanel.anchoredPosition.y >= offScreenPositionY)
        {
            // If the credits panel has moved off-screen, go to the main menu
            GoToMainMenuScene();
        }
    }

    public void SkipCredits()
    {
        if(!skipCredits)
        {
            scrollSpeed *= 10;
            skipCredits = true;
        }
        else
        {
            GoToMainMenuScene();
        }
    }

    private void GoToMainMenu() => fadeEffect.ScreenFade(1f, 1.5f, GoToMainMenuScene);

    private void GoToMainMenuScene()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
