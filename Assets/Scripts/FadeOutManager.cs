using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class FadeOutManager : MonoBehaviour {

    public Image fadeImage;
    public Image halfFadeImage;
    public float SpeedMultiplier = 1;

    private void Start()
    {
        fadeImage.enabled = false;
        fadeImage.color = new Color(1, 1, 1, 0);
        halfFadeImage.enabled = false;
        halfFadeImage.color = new Color(1, 1, 1, 0);
    }

    public void FadeOut()
    {
        //Debug.Log("Sleeping");
        fadeImage.enabled = true;
        StartCoroutine(FadeAway(true));
    }

    public void StartFade()
    {
        halfFadeImage.enabled = true;
        StartCoroutine(Fade(true));
    }
 
    IEnumerator FadeAway(bool fadeAway)
    {
        
        if (fadeAway)
        {
            // loop over 1 second 
            for (float i = 0; i <= 1; i += Time.deltaTime * SpeedMultiplier)
            {
                // alpha opaque
                fadeImage.color = new Color(0, 0, 0, i);
                yield return new WaitForSeconds (0.05f);
				
				if(i>=0.95)
                StartCoroutine(FadeAway(false));
            }
        }
        
        else
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime * SpeedMultiplier)
            {
                // alpha transparent
                fadeImage.color = new Color(0, 0, 0, i);
                yield return new WaitForSeconds(0.05f);

                if (i <= 0.01)
                {
					//Debug.Log("fade out test");
					//fadeImage.enabled = false;  //This isn't working yet :c					
                }
              
            }
			fadeImage.enabled = false; 
            GetComponent<Image>().enabled=false;
        }
    }


    IEnumerator Fade(bool fade)
    {

        if (fade)
        {
            // loop over 1 second 
            for (float i = 0; i <= 0.5; i += Time.deltaTime * SpeedMultiplier)
            {
                // alpha opaque
                halfFadeImage.color = new Color(0, 0, 0, i);
                yield return new WaitForSeconds(0.05f);

                if (i >= 0.45)
                    StartCoroutine(Fade(false));
            }
         
        }

    }
}
