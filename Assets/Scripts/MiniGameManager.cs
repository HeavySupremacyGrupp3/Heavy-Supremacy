using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public produktScript produktScript;

    public GameObject produktPrefab;
    private List<GameObject> produkter;

    angstStatScript StatReference;

    public Vector3 moveProduction;
    public float productInterval;
    float updateCounter = 0;
    bool spawnStuff = true;

    bool productsAreStopped = false;

    int stopCounter = 0;
    int finishedProducts = 0;

    public bool cantStopWontStop;

    [SerializeField]
    [Range(0, 1)]
    private float[] productChanse;


    [SerializeField]
    private float angstTick = 1f;
    [SerializeField]
    private int angstAmount = 1;
    private float angst;
    private float addedAngst = 0;

    [SerializeField]
    private Text angstText;

    [SerializeField]
    private Text moneyText;

    [HideInInspector]
    public List<GameObject> productList;
	
	public Transform checkpoint;

    int productsSeen = 0;

    [HideInInspector]
    public float addedMoney;
    private moneyStatScript moneyStats;

    [SerializeField]
    private int quitWorkAfterProducts;

    [SerializeField]
    private GameObject resultScreen;
	
	public delegate void mittEvent();
    public static event mittEvent stopEverything;

    void OnEnable()
    {
        produktScript.earnMoney += earnMoneyIfReachedEnd;
		produktScript.collidedWithBox += stopWorkIfEnoughProducts;
		produktScript.JustReachedCheckpoint += checkpointReached;
    }

    void OnDisable()
    {
        produktScript.earnMoney -= earnMoneyIfReachedEnd;
		produktScript.collidedWithBox -= stopWorkIfEnoughProducts;
		produktScript.JustReachedCheckpoint -=checkpointReached;
    }

	void checkpointReached()
	{
		Debug.Log("Checkpoint just reached.");
	}
	
    void Start()
    {
        AudioManager.instance.Play("rullband");
        AudioManager.instance.Play("hiss");
        StatReference = GameObject.Find("angstObject").GetComponent<angstStatScript>();
        moneyStats = GameObject.Find("moneyObject").GetComponent<moneyStatScript>();
        productList = new List<GameObject>();   //Skapar en lista 
        angst = StatReference.getAmount();
    }

    void Update()
    {
        angst += Time.deltaTime / angstTick;    //Gain x amount angst every x second (set in inspector)
        addedAngst += Time.deltaTime / angstTick;  //Same but this only keeps track on how much you've gained, not the total amount of angst
        StatReference.setAmount(Mathf.RoundToInt((angst) * angstAmount));   //Set the amount stat to angst (angstAmount is x angst per second´in inspector)

        updateCounter += Time.deltaTime;

        if (spawnStuff && updateCounter >= productInterval) //updateCounter%100==99 and int
        {
            updateCounter = 0;

            bool hasSpawned = false;
            float rnd = Random.value;
            float chance = 0f;
            for (int i = 1; i < productChanse.Length; i++)    //Går igenom varje produkttyps chans att spawna från en lista
            {
                chance += productChanse[i];                 // Exempel: 0+0.6 -> 0.6+0.2 -> 0.8+0.1 -> 0.9+0.1
                if (rnd < chance && hasSpawned == false)   //Om ett tal mellan 0-1 är mindre än produktchansen (sätts i inspektorn) och om inget annat har spawnat
                {
					if(i<productChanse.Length-1)
						SpawnProduct(i);                //Spawna den produkt som for-satsen var på i listan som mötte kraven (talet var mindre än chansen)

					hasSpawned = true;                  //Spawna inget mer förrän updateCounter möter conditions igen och processen börjar om
                }
            }
            if (!hasSpawned)
                SpawnProduct(0);                    //Om inget i listan spawnade, spawna metallklumpen

            productsSeen++;

            if (productsSeen > 1)
                changeSpawnStopProducts();
        }

        if (productsAreStopped)
        {
            stopCounter++;
            if (stopCounter == 50)
            {
                changeSpawnStopProducts();
                stopCounter = 0;
            }
        }
    }

    public void LoadHUB()
    {
        SceneManager.LoadScene("HUBScene");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("WorkTutorialScene");
    }

    public void LoadWork()
    {
        SceneManager.LoadScene("WorkScene");
    }

    public void LoadResultScreen()
    {
        Time.timeScale = 0;     //Pause the game/production
        angstText.text = "+" + Mathf.RoundToInt(addedAngst);  //Show how much angst you've gained
        moneyText.text = "+" + addedMoney;       //Show how much money you earned
        resultScreen.SetActive(true);                          //activate the resultScreen
    }

    //switches spawning on/off, stops/moves products
    void changeSpawnStopProducts()
    {
        spawnStuff = !spawnStuff;
        productsAreStopped = !productsAreStopped;
		stopEverything();
    }

    //counts products finished, and shows the result screen if products finished > set amount
    void earnMoneyIfReachedEnd()
    {
		addedMoney += moneyStats.difference; //When a product is finished, collect data on how much money you earned (difference)
    }
	
	void stopWorkIfEnoughProducts()
	{
		finishedProducts++;
		if (finishedProducts == quitWorkAfterProducts && !cantStopWontStop)
			LoadResultScreen();   //Load resultScreen when the number of finished products has been achieved
	}

    public void SpawnProduct(int rng)
    {
        GameObject nyProdukt = Instantiate(produktPrefab, moveProduction, Quaternion.identity);  //Instantierar en produktprefab på en plats i rymden
        //rng = Random.Range(0, produktScript.Sprites.Length);
        nyProdukt.GetComponent<produktScript>().type = rng;                               //Produktscriptet på produkten har variabeln "type" som sätts efter den int som skickas med
        SpriteRenderer sr = nyProdukt.GetComponent<SpriteRenderer>();                     //hämtar den instantierade produktens spriterenderer
        sr.sprite = produktScript.Sprites[rng];                                           //Sätter spriten på produkten till samma som typen på produktskriptet
        productList.Add(nyProdukt);                                                   //Lägger till instantierad produkt i listan
    }

    public void RemoveFromList()
    {
        productList.RemoveAt(0);     //Tar bort första produkten i listan (Den som lades  till först är också produkten som når slutet av skärmen och ska tas bort först)
    }


}
