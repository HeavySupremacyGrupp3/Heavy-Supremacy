using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class workTutorialManager : MonoBehaviour {

	//public MiniGameManager targetScript;   
    public GameManager gmScript;
    public produktScript produktScript;

    public GameObject produktPrefab;
    private List<GameObject> produkter;
	
	angstStatScript StatReference;

    public Vector3 moveProduction;
    public float productInterval;
	float updateCounter=0;
	

    public Sprite[] productSprites;
    //private GameObject product;
	
	public delegate void mittEvent();
	public static event mittEvent stopProducts;
	
	public int unlockedTypes;
	
	int stopCounter=0;
	int finishedProducts=0;
	int toiletCounter=0;
	
	
	public bool cantStopWontStop;
	public bool stayUntilCompleted;
	public bool spawnFlaskor;
	public bool gameOver;
	
	bool myBool=false;	
	bool spawnStuff=true;
	bool productsAreStopped=false;
	
	public void ToggleScript(MonoBehaviour m)
	{
		m.enabled=!m.enabled;
	}
	
	public void ToggleClassicWorkScrip()
	{
		//targetScript.SetActive(!targetScript.active);
		myBool=!myBool;
		//targetScript.enabled=myBool;
	}
	
	public void ToggleGameObject(GameObject target)
    {
		//Debug.Log("AAAAAAAAARGHHH!");
        target.SetActive(!target.active);
    }

    void OnEnable()
	{
		produktScript.earnMoney +=omaewashindeiru;
		TimingMachine.productHit += toiletClogger;
		unhideTextScript.unhideText+=ToggleGameObject;
	}
	
	void OnDisable()
	{
		produktScript.earnMoney -= omaewashindeiru;
		TimingMachine.productHit -= toiletClogger;
		unhideTextScript.unhideText+=ToggleGameObject;
	}
	
	void toiletClogger()
	{
		toiletCounter++;
		Debug.Log("toilet clogger "+toiletCounter);
		if(toiletCounter==unlockedTypes)
		{
			changeSpawnStopProducts();
			toiletCounter=0;
		}
	}
	
	void changeSpawnStopProducts()
	{
		spawnStuff=!spawnStuff;
		if(!stayUntilCompleted)
			productsAreStopped=!productsAreStopped;
		stopProducts();
	}
	
	public void spawnMetallklump()
	{
		updateCounter=0;
		GameObject nyProdukt = Instantiate(produktPrefab, moveProduction, Quaternion.identity);	
		//changeSpawnStopProducts();
	}
	
	public void spawnTomFlaska()
	{
		GameObject nyProdukt = Instantiate(produktPrefab, moveProduction, Quaternion.identity);
		nyProdukt.GetComponent<produktScript>().type = 1;
        SpriteRenderer sr = nyProdukt.GetComponent<SpriteRenderer>();
        sr.sprite = productSprites[1];
	}
	
	void omaewashindeiru()
	{
		finishedProducts++;
		
		if(finishedProducts==5 && cantStopWontStop==false)
		{
			LoadHUB();
		}
	}

    void Start ()
	{
        gmScript = GetComponent<GameManager>();
        produktScript = GetComponent<produktScript>();
        StatReference =GameObject.Find("angstObject").GetComponent<angstStatScript>();
        //AudioManager.instance.Play("Atmosphere"); //blir trött i huvet
    }	
	
	void Update ()
	{
		if(spawnFlaskor && updateCounter >= productInterval)
		{
			updateCounter=0;
			spawnTomFlaska();
		}

        if (spawnStuff)
        updateCounter += Time.deltaTime;
			
		if(!spawnFlaskor && spawnStuff && updateCounter >= productInterval) //updateCounter%100==99 and int
        {

            updateCounter=0;
            //Add new gameobject with a random sprite
            GameObject nyProdukt = Instantiate(produktPrefab, moveProduction, Quaternion.identity);
			
			int leastOf;
			
			if(productSprites.Length<unlockedTypes)
				leastOf=productSprites.Length;
			else
				leastOf=unlockedTypes;
			
			Debug.Log("least of "+leastOf);
            int rng = Random.Range(0, leastOf);
            nyProdukt.GetComponent<produktScript>().type = rng;
            SpriteRenderer sr = nyProdukt.GetComponent<SpriteRenderer>();
            sr.sprite = productSprites[rng];
			changeSpawnStopProducts();
			
			
        }	
		
		if(!stayUntilCompleted && productsAreStopped)
		{
			stopCounter++;
			if(stopCounter==50)
			{
				changeSpawnStopProducts();
				stopCounter=0;
			}
		}
		
		if(stayUntilCompleted && productsAreStopped && !spawnFlaskor)
		{
			changeSpawnStopProducts();
		}
	}
	
	public void QuitWork()
	{
		StatReference.addOrRemoveAmount(15f);
	}
	
	public void changeStopRequirements()
	{
		stayUntilCompleted=!stayUntilCompleted;
	}
	
	public void changeSpawnaFlaskor()
	{
		spawnFlaskor=!spawnFlaskor;
	}
	
	public void setUnlockedTypes(int t)
	{
		unlockedTypes=t;
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
    }
	
	public void LoadWork()
    {
		SceneManager.LoadScene("WorkScene");
    }

   public void MenuIsClicked()
    {
        spawnStuff = false;
    }
	
}
