using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class FadeOutManager : MonoBehaviour {

    public Image fadeTransform;

    public void OnButtonClick()
    {
        StartCoroutine(FadeAway(true));
    }
 
    IEnumerator FadeAway(bool fadeAway)
    {
        
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // alpha transparent
                fadeTransform.color = new Color(1, 1, 1, 0);
                yield return null;
            }
        }
        
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // alpha full
                fadeTransform.color = new Color(1, 1, 1, 1);
                yield return null;
            }
        }
    }
}
