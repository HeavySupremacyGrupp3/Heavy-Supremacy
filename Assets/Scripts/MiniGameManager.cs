using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour {

    public bool gameOver;
    public GameManager gmScript; 
	
	public Object HUBScene;
	public Object WorkScene;
	
	public GameObject ProduktPrefab;
	private List<GameObject> produkter;
	
	happinessStatScript StatReference;
	
	int updateCounter=0;

    void Start ()
	{
        gmScript = GetComponent<GameManager>();
		StatReference=GameObject.Find("happinessObject").GetComponent<happinessStatScript>();
	}	
	
	void Update ()
	{
		/*
		if(Time.fixedTime%3==2)	
		{
			StatReference.addOrRemoveAmount(-20f);
			GameObject nyProdukt = (GameObject)Instantiate (ProduktPrefab, transform.position, transform.rotation);
			produkter.Add(nyProdukt);		
		}
		*/
		
		updateCounter++;
		if(updateCounter%100==99)	
		{
			updateCounter=0;
			StatReference.addOrRemoveAmount(-0.05f);
			GameObject nyProdukt = (GameObject)Instantiate (ProduktPrefab, transform.position, transform.rotation);
			produkter.Add(nyProdukt);
			
		}
		//Debug.Log(updateCounter);
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
	
	public void LoadWork()
    {
		SceneManager.LoadScene(WorkScene.name);
        //gmScript.LoadHUB();
    }

   
}
