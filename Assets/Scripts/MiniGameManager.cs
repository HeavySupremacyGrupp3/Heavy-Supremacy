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
	int finishedProducts=0;
		int toiletCounter=0;
	public int unlockedTypes;
	
	public bool cantStopWontStop;
	public bool stayUntilCompleted;
	public bool spawnFlaskor;

    [SerializeField]
    private float angstTick = 1f;
    [SerializeField]
    private int angstAmount = 1;
    private float angst = 0f;
	
	bool inTheNameOfTheLaw=false;

	int productsSeen=0;

    void OnEnable()
	{
		produktScript.earnMoney +=omaewashindeiru;
		TimingMachine.productHit += toiletClogger;
		TimingMachine.productCompared += chickPeas; 
		//machineBelowDetectorScript.productBelowDetected += chickPeas;
	}
	
	void OnDisable()
	{
		produktScript.earnMoney -=omaewashindeiru;
		TimingMachine.productHit -= toiletClogger;
		TimingMachine.productCompared -= chickPeas; 
		//machineBelowDetectorScript.productBelowDetected -= chickPeas;
	}
	
	void toiletClogger()
	{
		toiletCounter++;
		Debug.Log("toilet clogger "+toiletCounter+", goal "+unlockedTypes);
		
		if(toiletCounter==unlockedTypes)
		{
			Debug.Log("Successfully unclogged!");
			changeSpawnStopProducts();
			//inTheNameOfTheLaw=false;
			toiletCounter=0;
		}
	}
	
	void chickPeas(bool b)
	{
		 if(productsSeen>1 && b==true)
		 {
			 Debug.Log("Chickpeas");
			 toiletCounter++;
			 if(toiletCounter==unlockedTypes)
			 {
				 inTheNameOfTheLaw=false;
				 toiletCounter=0;
			 }
			 //inTheNameOfTheLaw=!inTheNameOfTheLaw;
			 //changeSpawnStopProducts();
		 }
		 else if(b==false)
		 {
			 inTheNameOfTheLaw=true;
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
		productsSeen++;
	}
	
	public void spawnTomFlaska()
	{
		GameObject nyProdukt = Instantiate(produktPrefab, moveProduction, Quaternion.identity);
		nyProdukt.GetComponent<produktScript>().type = 1;
        SpriteRenderer sr = nyProdukt.GetComponent<SpriteRenderer>();
        sr.sprite = productSprites[1];
		productsSeen++;
	}
	
	void omaewashindeiru()
	{
		finishedProducts++;
		
		if(finishedProducts==10 && cantStopWontStop==false)
		{
			LoadHUB();
		}
	}

    void Start ()
	{
        gmScript = GetComponent<GameManager>();
        produktScript = GetComponent<produktScript>();
        StatReference = GameObject.Find("angstObject").GetComponent<angstStatScript>();
        //AudioManager.instance.Play("Atmosphere"); //blir trött i huvet
    }	
	
	void Update ()
	{		
		if(spawnFlaskor && updateCounter >= productInterval)
		{
			updateCounter=0;
			spawnTomFlaska();
		}
     
        angst += Time.deltaTime / angstTick;
        StatReference.setAmount(Mathf.RoundToInt((angst) * angstAmount));


        if (spawnStuff) //&& !spawnFlaskor
        updateCounter += Time.deltaTime;
			
		if(!spawnFlaskor && spawnStuff && updateCounter >= productInterval) //updateCounter%100==99 and int
        {
			//JULIA KOLLA HÄR VAD TYCKER DU?
			int leastOf;
			
			if(productSprites.Length<unlockedTypes)
				leastOf=productSprites.Length;
			else
				leastOf=unlockedTypes;
			
			Debug.Log("least of "+leastOf);
            int rng = Random.Range(0, leastOf);
			

            updateCounter=0;
            //Add new gameobject with a random sprite
			
			float metalChance=0.4f;
			float burkChance=0.3f;
			float mousseChance=0.2f;
			float canChance=0.1f;
			
            
			if (Random.value >= metalChance)
            {
                GameObject nyProdukt = Instantiate(produktPrefab, moveProduction, Quaternion.identity);
                rng = Random.Range(0, productSprites.Length);
                nyProdukt.GetComponent<produktScript>().type = rng;
                SpriteRenderer sr = nyProdukt.GetComponent<SpriteRenderer>();
                sr.sprite = productSprites[rng];
                //changeSpawnStopProducts();
            }
            else if(Random.value >= burkChance)  //Add new gameobject with a random sprite but not the full can
            {
                GameObject nyProdukt = Instantiate(produktPrefab, moveProduction, Quaternion.identity);
                rng = Random.Range(0, 2);
                nyProdukt.GetComponent<produktScript>().type = rng;
                SpriteRenderer sr = nyProdukt.GetComponent<SpriteRenderer>();
                sr.sprite = productSprites[rng];
                //changeSpawnStopProducts();
            }
			
			 else if(Random.value >= mousseChance)  //Add new gameobject with a random sprite but not the full can
            {
                GameObject nyProdukt = Instantiate(produktPrefab, moveProduction, Quaternion.identity);
                rng = Random.Range(0, 2);
                nyProdukt.GetComponent<produktScript>().type = rng;
                SpriteRenderer sr = nyProdukt.GetComponent<SpriteRenderer>();
                sr.sprite = productSprites[rng];
                //changeSpawnStopProducts();
            }
			productsSeen++;
			if(productsSeen>1 && inTheNameOfTheLaw)
				changeSpawnStopProducts();
			//inTheNameOfTheLaw=true;
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
	
	public void changeStopRequirements()
	{
		stayUntilCompleted=!stayUntilCompleted;
	}
	
	public void changeSpawnaFlaskor()
	{
		//updateCounter=0;
		//spawnFlaskorJustChanged=1;
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

    /*public IEnumerator AngstTick()
    {
        yield return new WaitForSeconds(angstTick);
        for (float i = 0; i <= angstTick + 1; i += Time.deltaTime)
        {
            if (i >= angstTick)
            {
                StatReference.addOrRemoveAmount(angstAmount);
            }
            yield return new WaitForSeconds(angstTick);
        }

    }
    */
}
