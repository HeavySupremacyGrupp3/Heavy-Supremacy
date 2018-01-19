using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour {

    public bool gameOver;

    void Start()
    {
        Initialize();
    }

    private void Update()
    {
        GameOver();
    }


    void Initialize()
    {

    }

    public void GameOver()
    {

    }


    public void ReloadScene()
    {
        if (gameOver) //&& Input.
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
}
