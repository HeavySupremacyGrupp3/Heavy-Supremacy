using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour {

    public bool gameOver;
    public GameManager gmScript; 
	
	public Object HUBScene;
	
	public GameObject ProduktPrefab;
	private List<GameObject> produkter;

    void Start ()
	{
        gmScript = GetComponent<GameManager>();
	}	
	
	void Update ()
	{
		if(Time.fixedTime%3==0)	
		{
			GameObject nyProdukt = (GameObject)Instantiate (ProduktPrefab, transform.position, transform.rotation);
			produkter.Add(nyProdukt);
		}
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

    public void LoadHUB()
    {
		SceneManager.LoadScene(HUBScene.name);
        //gmScript.LoadHUB();
    }

   
}
