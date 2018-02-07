using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

    public void LoadHUB()
    {
        SceneManager.LoadScene("HUBScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
