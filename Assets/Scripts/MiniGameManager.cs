using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour {

    public bool gameOver;
    public GameManager gmScript;
    public produktScript produktScript;

    public GameObject produktPrefab;
    private List<GameObject> produkter;
	
	angstStatScript StatReference;

    public Vector3 moveProduction;
    public float productInterval;
	float updateCounter=0;
	bool spawnStuff=true;

    public Sprite[] productSprites;
    //private GameObject product;
	
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
        produktScript = GetComponent<produktScript>();
        StatReference =GameObject.Find("angstObject").GetComponent<angstStatScript>();
		stopCounter=-90;

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
        if (spawnStuff)
        updateCounter += Time.deltaTime;
		
		if(spawnStuff && updateCounter >= productInterval) //updateCounter%100==99 and int
        {

            updateCounter = 0;

            //Add new gameobject with a random sprite
            GameObject nyProdukt = Instantiate(produktPrefab, moveProduction, Quaternion.identity);
            int rng = Random.Range(0, productSprites.Length);
            nyProdukt.GetComponent<produktScript>().type = rng;
            SpriteRenderer sr = nyProdukt.GetComponent<SpriteRenderer>();
            sr.sprite = productSprites[rng];
        }
		
		if(productsAreStopped)
		{
			stopCounter++;
			if(stopCounter==50)
			{
				changeSpawnStopProducts();
				stopCounter=0;
			}
		}
		
		stopCounter++;
		if(stopCounter==120)
		{
			changeSpawnStopProducts();
			stopCounter=0;
		}
		
		//Debug.Log(updateCounter);
	}
	
	public void QuitWork()
	{
		StatReference.addOrRemoveAmount(15f);
	}

	//se timingMachine
    public void Collided(GameObject productObject)
    {
        produktScript newProduct = productObject.GetComponent<produktScript>();
        SpriteRenderer sr = productObject.GetComponent<SpriteRenderer>();
        int type = newProduct.GetComponent<produktScript>().type;

        switch (type)
        {
            case 0:
                type++;
                sr.sprite = productSprites[type];
                break;
            case 1:
                type++;
                sr.sprite = productSprites[type];
                break;
            case 2:
                type++;
                sr.sprite = productSprites[type];
                break;
            case 3:
                type++;
                sr.sprite = productSprites[type];
                break;
            case 4:
                break;
        }

        newProduct.type = type;
    }

    public void ChangeProduct()
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

    public void LoadHUB()
    {
		QuitWork();
		SceneManager.LoadScene("HUBScene");
        //gmScript.LoadHUB();
    }
	
	public void LoadWork()
    {
		SceneManager.LoadScene("WorkScene");
        //gmScript.LoadHUB();
    }

   public void MenuIsClicked()
    {
        spawnStuff = false;
    }
}
