using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class FadeOutManager : MonoBehaviour
{

    public Image fadeImage;
    public Image halfFadeImage;
    public float SpeedMultiplier = 1;
    public float FadeInSeconds = 1f;
    private float TimeLeft = 0;
    private Action HalfwayCallback = null;

    private void Start()
    {
        fadeImage.enabled = false;
        fadeImage.color = new Color(1, 1, 1, 0);
        if (halfFadeImage != null)
        {
            halfFadeImage.enabled = false;
            halfFadeImage.color = new Color(1, 1, 1, 0);
        }
    }

    public void FadeOut(bool makeALoop = true, Action callback = null)
    {
        HalfwayCallback = callback;
        fadeImage.enabled = true;
        StartCoroutine(FadeAway(true, makeALoop));
    }

    IEnumerator FadeAway(bool fadeAway, bool makeALoop = true)
    {

        TimeLeft = FadeInSeconds;

        if (fadeAway)
        {
            // loop over 1 second 
            while ((TimeLeft -= Time.deltaTime) > 0)
            //for (float i = 0; i <= 1; i += Time.deltaTime * SpeedMultiplier)
            {
                // alpha opaque
                fadeImage.color = new Color(0, 0, 0, 1-(TimeLeft/FadeInSeconds));
                yield return null;

                if (TimeLeft <= 0.05f && makeALoop)
                {
                    fadeImage.color = new Color(0, 0, 0, 1);
                    HalfwayCallback();
                    StartCoroutine(FadeAway(false));
                    break;
                }
            }
        }

        else
        {
            // loop over 1 second backwards
            while ((TimeLeft -= Time.deltaTime) > 0)
            //for (float i = 1; i > 0; i -= Time.deltaTime * SpeedMultiplier)
            {
                // alpha transparent
                fadeImage.color = new Color(0, 0, 0, TimeLeft/FadeInSeconds);
                yield return null;

                if (TimeLeft <= 0.05f)
                {
                    fadeImage.color = new Color(0, 0, 0, 0);

                    break;	
                }

            }
            fadeImage.enabled = false;
            GetComponent<Image>().enabled = false;
        }
    }
}
