using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements; // Changed type from GameManager[] to GameObject[]
    [SerializeField] private GameObject continueButton;
    private UI_FadeEffect fadeEffect;
    public string sceneToLoad; // Name of the scene to load  


    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        if(IsContinueLevelAvailable())
        {
            continueButton.SetActive(true); // Show continue button if a level is available to continue  
        }
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

    public void ContinueGame()
    {
        int continueLevel = PlayerPrefs.GetInt("ContinueLevelNumber", 0);
        SceneManager.LoadScene("Level_" + continueLevel); // Load the last saved level

    }

    private void LoadLevelScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private bool IsContinueLevelAvailable()
    {
        return PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;
    }
}
