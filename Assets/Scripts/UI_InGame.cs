using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame Instance;
    public UI_FadeEffect fadeEffect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }

        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        // Optionally, you can start with a fade-in effect when the in-game UI is loaded
        if (fadeEffect != null)
        {
            fadeEffect.ScreenFade(0f, 1.5f); // Fade in to transparent over 1.5 seconds
        }
    }
}
