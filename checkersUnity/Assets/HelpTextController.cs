using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpTextController : MonoBehaviour
{
    public bool somenteInicio;
    TextMeshProUGUI textToUse;
    int timeMultiplier = 1;
    // Start is called before the first frame update
    void Start()
    {
        textToUse = GetComponent<TextMeshProUGUI>();
        //StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<TextMeshProUGUI>()));
        StartCoroutine(IntroFade(textToUse));
    }

    // Update is called once per frame
    void Update()
    {
        if (!somenteInicio && Input.anyKeyDown)
        {
            if (!Input.GetKeyDown("escape") && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
            {
                Debug.Log("Apertou alguma tecla");
                StartCoroutine(IntroFade(textToUse));
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<TMPro>()));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<TMPro>()));
        }
        */
    }


    /*
    public IEnumerator FadeTextToFullAlpha(float t, TMPro i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TMPro i)
    {
        Debug.Log("FadeOut  i = " +  i);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
    */

    private IEnumerator FadeInText(float timeSpeed, TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }
    private IEnumerator FadeOutText(float timeSpeed, TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }
    public void FadeInText(float timeSpeed = -1.0f)
    {
        if (timeSpeed <= 0.0f)
        {
            timeSpeed = timeMultiplier;
        }
        StartCoroutine(FadeInText(timeSpeed, textToUse));
    }
    public void FadeOutText(float timeSpeed = -1.0f)
    {
        if (timeSpeed <= 0.0f)
        {
            timeSpeed = timeMultiplier;
        }
        StartCoroutine(FadeOutText(timeSpeed, textToUse));
    }

    private IEnumerator IntroFade(TextMeshProUGUI textToUse)
    {
        yield return StartCoroutine(FadeInText(1f, textToUse));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOutText(1f, textToUse));
        //End of transition, do some extra stuff!!
    }
}
