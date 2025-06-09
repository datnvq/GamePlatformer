using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeEffect : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    public void ScreenFade(float targetAlpha, float duration, System.Action onComplete = null)
    {
         StartCoroutine(FadeCoroutine(targetAlpha, duration, onComplete)); // Fade to black over 1 second
    }

    private IEnumerator FadeCoroutine(float targetAlpha, float duration, System.Action onComplete)
    {
        float time = 0;
        Color curentColor = fadeImage.color;

        float startAlpha = curentColor.a;

        while(time < duration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);

            fadeImage.color = new Color(curentColor.r, curentColor.g, curentColor.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(curentColor.r, curentColor.g, curentColor.b, targetAlpha); // Ensure final alpha is set

        onComplete?.Invoke(); // Invoke the callback if provided
    }
}
