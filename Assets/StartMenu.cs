using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

    private void Start()
    {
        AudioManager.instance.Play("HUBMusic");
    }

    public void LoadHUB()
    {
        GameManager.RestartGame = true;
        SceneManager.LoadScene("HUBScene");
    }

    public void LoadTutorial()
    {
        GameManager.RestartGame = true;
        SceneManager.LoadScene("WorkTutorialScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
