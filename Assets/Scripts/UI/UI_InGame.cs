using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame Instance;
    public UI_FadeEffect fadeEffect;

    private void Awake()
    {
        Instance = this;

        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        fadeEffect.ScreenFade(0f, 1.5f); // Fade in to transparent over 1.5 seconds
    }
}
