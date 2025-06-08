using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements; // Changed type from GameManager[] to GameObject[]  
    private UI_FadeEffect fadeEffect;
    public string sceneToLoad = "GameScene"; // Name of the scene to load  

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        // Optionally, you can start with a fade-in effect when the main menu is loaded  
            fadeEffect.ScreenFade(0f, 1.5f); // Fade in to transparent over 1.5 second  
    }

    public void SwitchUI(GameObject uiToEnable)
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        uiToEnable.SetActive(true); // Added logic to enable the specified UI element  
    }

    public void NewGame()
    {
        // Load the specified scene when the button is clicked  
        fadeEffect.ScreenFade(1f, 1.5f, LoadLevelScene); // Fade to black over 1.5 seconds  
    }

    private void LoadLevelScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
