using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame Instance;

    [SerializeField] private TextMeshProUGUI fruitText;
    [SerializeField] private TextMeshProUGUI timerText;
    public UI_FadeEffect fadeEffect { get; private set; }

    private void Awake()
    {
        Instance = this;

        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        fadeEffect.ScreenFade(0f, 1.5f); // Fade in to transparent over 1.5 seconds
    }

    public void UpdateFruitText(int fruitCount, int totalFruit)
    {
        fruitText.text = fruitCount + " / " + totalFruit;
    }

    public void UpdateTimerText(float time)
    {
        timerText.text = Mathf.FloorToInt(time).ToString("00") + "s";
    }
}
