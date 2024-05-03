using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderFadeOut : MonoBehaviour
{
    public Slider slider;
    public float fadeDuration = 1f;

    // Call this method to start the fade-out process
    public void StartFadeOut()
    {
        StartCoroutine(FadeOutSlider());
    }

    IEnumerator FadeOutSlider()
    {
        // Get the CanvasGroup component of the slider
        CanvasGroup canvasGroup = slider.GetComponent<CanvasGroup>();

        // Ensure the CanvasGroup exists and the alpha is initially set to 1
        if (canvasGroup == null)
            canvasGroup = slider.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;

        // Calculate the rate of alpha decrease per second
        float alphaDecreaseRate = 1f / fadeDuration;

        // Gradually decrease the alpha until it reaches 0
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= alphaDecreaseRate * Time.deltaTime;
            yield return null;
        }

        // Disable the slider after fading out
        slider.gameObject.SetActive(false);
    }
}
