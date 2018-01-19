using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour {

    private bool gameOver;

    void Start () {
		
	}
	
	
	void Update () {
        GameOver();
	}

    public void GameOver()
    {
        gameOver = true;
        ReloadScene();
    }

    public void ReloadScene()
    {
        if (gameOver) //&& Input.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
