using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDownAnimation : MonoBehaviour
{
    public float timeOfTravel = 0.75f;

    float currentTime;
    float normalizedValue;
    RectTransform rectTransform;

    private void OnEnable()
    {
        currentTime = 0;

        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, Screen.height);

        StartCoroutine(DoAnimation());
    }

    IEnumerator DoAnimation()
    {
        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel;

            rectTransform.anchoredPosition = new Vector2(0, EaseOutCubic(Screen.height, 0, normalizedValue));

            yield return new WaitForEndOfFrame();
        }
    }

    public static float EaseOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }
}
