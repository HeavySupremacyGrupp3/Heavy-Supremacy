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
	bool spawnStuff=true;
	
	public delegate void mittEvent();
	public static event mittEvent stopProducts;
	
	bool productsAreStopped=false;
	
	int stopCounter=0;
	
	void OnEnable()
	{
		TimingMachine.productDetected +=changeSpawnStopProducts;
	}
	
	void OnDisable()
	{
		TimingMachine.productDetected -=changeSpawnStopProducts;
	}
	
	void changeSpawnStopProducts()
	{
		spawnStuff=!spawnStuff;
		productsAreStopped=!productsAreStopped;
		stopProducts();
	}

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
		if(spawnStuff && updateCounter%100==99)	
		{
			updateCounter=0;
			StatReference.addOrRemoveAmount(0.05f);
			GameObject nyProdukt = (GameObject)Instantiate (ProduktPrefab, transform.position, transform.rotation);
			//produkter.Add(nyProdukt);
			
		}
		
		if(productsAreStopped)
		{
			stopCounter++;
			//Debug.Log(stopCounter);
			if(stopCounter==50)
			{
				changeSpawnStopProducts();
				stopCounter=0;
			}
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
