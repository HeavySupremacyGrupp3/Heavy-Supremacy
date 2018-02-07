using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

    public void LoadHUB()
    {
        GameManager.RestartGame = true;
        SceneManager.LoadScene("HUBScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
