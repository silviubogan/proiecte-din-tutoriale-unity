using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CountdownText : MonoBehaviour
{
    public int startSeconds = 3;
    public GameObject toBeShownAfter;

    Text countdownText;

    void Awake()
    {
        countdownText = GetComponent<Text>();
    }

    void OnEnable()
    {
        // aici eventual reseteaza starea jocului in caz ca nivelul
        // anterior jucat este inca vizibil pe ecran in spatele de
        // exemplu al meniurilor:
        toBeShownAfter.SetActive(false);

        countdownText.text = startSeconds.ToString();
        countdownText.enabled = true;

        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        for (int i = startSeconds; i >= 1; --i)
        {
            countdownText.text = i.ToString();
            StartCoroutine(FadeTextToFullAlpha(1f, countdownText));

            yield return new WaitForSeconds(1);
        }

        // aici se apeleaza cod sa fie executat dupa ce s-a ajuns la 0:
        gameObject.SetActive(false);
        toBeShownAfter.SetActive(true);
    }

    IEnumerator FadeTextToFullAlpha(float t, Text txt)
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        yield return null;

        while (txt.color.a < 1.0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public void WhenButtonPressed()
    {
        gameObject.SetActive(true);
    }
}
