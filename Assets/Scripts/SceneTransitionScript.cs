using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionScript : MonoBehaviour {


    public float LoadDelay = 1;
    public float FadeInSeconds = 1;

    public void StartTransition(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        FadeOutManager fadeOut = FindObjectOfType<FadeOutManager>();
        fadeOut.FadeInSeconds = FadeInSeconds;
		Time.timeScale = 1;

        fadeOut.FadeOut(false);

        yield return new WaitForSeconds(LoadDelay);

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
