using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gigManagerTest : MonoBehaviour {

    public bool gameOver;
    //public GameManager gmScript; 
	
	public Object WorkScene;

    void Start ()
	{
        //gmScript = GetComponent<GameManager>();
	}	
	
	void Update ()
	{

	}

    public void GameOver()
    {
        gameOver = true;
        ReloadScene();
    }

    public void ReloadScene()
    {
        if (gameOver) //TODO: && Input.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadWork()
    {
		SceneManager.LoadScene(WorkScene.name);
        //gmScript.LoadHUB();
    }

   
}
