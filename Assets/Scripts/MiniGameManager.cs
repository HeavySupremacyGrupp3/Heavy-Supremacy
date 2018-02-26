using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{

    public bool gameOver;
    public GameManager gmScript;
    public produktScript produktScript;

    public GameObject produktPrefab;
    private List<GameObject> produkter;

    angstStatScript StatReference;

    public Vector3 moveProduction;
    public float productInterval;
    float updateCounter = 0;
    bool spawnStuff = true;

    //public Sprite[] produktScript.Sprites;
    //private GameObject product;

    public delegate void mittEvent();
    public static event mittEvent stopProducts;

    bool productsAreStopped = false;

    int stopCounter = 0;
    int finishedProducts = 0;
    int toiletCounter = 0;
    public int unlockedTypes;

    public bool cantStopWontStop;
    public bool stayUntilCompleted;
    public bool spawnFlaskor;

    [SerializeField]
    [Range(0, 1)]
    private float[] productChanse;


    [SerializeField]
    private float angstTick = 1f;
    [SerializeField]
    private int angstAmount = 1;
    private float angst = 0f;

    [HideInInspector]
    public List<GameObject> productList;

    bool inTheNameOfTheLaw = false;

    int productsSeen = 0;

    [SerializeField]
    private GameObject resultScreen;

    void OnEnable()
    {
        produktScript.earnMoney += omaewashindeiru;
        TimingMachine.productHit += toiletClogger;
        TimingMachine.productCompared += chickPeas;
        //machineBelowDetectorScript.productBelowDetected += chickPeas;
    }

    void OnDisable()
    {
        produktScript.earnMoney -= omaewashindeiru;
        TimingMachine.productHit -= toiletClogger;
        TimingMachine.productCompared -= chickPeas;
        //machineBelowDetectorScript.productBelowDetected -= chickPeas;
    }

    void toiletClogger()
    {
        toiletCounter++;
        Debug.Log("toilet clogger " + toiletCounter + ", goal " + unlockedTypes);

        if (toiletCounter >= unlockedTypes)
        {
            //Debug.Log("Successfully unclogged!");
            changeSpawnStopProducts();
            //inTheNameOfTheLaw=false;
            toiletCounter = 0;
        }
    }

    void chickPeas(bool b)
    {
        Debug.Log("the receiving end");
        if (productsSeen >= unlockedTypes && b == true) //productsSeen>1 && 
        {
            //Debug.Log("Chickpeas");
            toiletCounter++;
            if (toiletCounter >= unlockedTypes)
            {
                inTheNameOfTheLaw = false;
                toiletCounter = 0;
            }

            else
            {
                // inTheNameOfTheLaw=true;
            }
            //inTheNameOfTheLaw=!inTheNameOfTheLaw;
            //changeSpawnStopProducts();
        }
        else if (b == false)
        {
            inTheNameOfTheLaw = true;
        }
    }

    void changeSpawnStopProducts()
    {
        spawnStuff = !spawnStuff;
        if (!stayUntilCompleted)
            productsAreStopped = !productsAreStopped;
        //  stopProducts();
    }

    /*public void spawnMetallklump()
	{
		updateCounter=0;
		GameObject nyProdukt = Instantiate(produktPrefab, moveProduction, Quaternion.identity);
		productsSeen++;
	}*/

    /*public void spawnTomFlaska()
	{
		GameObject nyProdukt = Instantiate(produktPrefab, moveProduction, Quaternion.identity);
		nyProdukt.GetComponent<produktScript>().type = 1;
        SpriteRenderer sr = nyProdukt.GetComponent<SpriteRenderer>();
        sr.sprite = produktScript.Sprites[1];
		productsSeen++;
	}*/

    void omaewashindeiru()
    {
        finishedProducts++;

        if (finishedProducts == 20 && cantStopWontStop == false)
        {
            resultScreen.SetActive(true);
            //LoadHUB(); We want to show the resultscreen, not load hub c;
        }
    }

    void Start()
    {
        AudioManager.instance.Play("rullband");
        AudioManager.instance.Play("hiss");
        //gmScript = GetComponent<GameManager>();
        gmScript = FindObjectOfType<GameManager>();
        //produktScript = GetComponent<produktScript>();
        StatReference = GameObject.Find("angstObject").GetComponent<angstStatScript>();
        productList = new List<GameObject>();   //Skapar en lista 
    }

    void Update()
    {
        if (spawnFlaskor && updateCounter >= productInterval)
        {
            updateCounter = 0;
            //spawnTomFlaska();
        }

        angst += Time.deltaTime / angstTick;
        StatReference.setAmount(Mathf.RoundToInt((angst) * angstAmount));


        if (spawnStuff) //&& !spawnFlaskor
            updateCounter += Time.deltaTime;

        if (!spawnFlaskor && spawnStuff && updateCounter >= productInterval) //updateCounter%100==99 and int
        {

            updateCounter = 0;


            bool hasSpawned = false;
            for (int i = 1; i < productChanse.Length; i++)    //Går igenom varje produkttyps chans att spawna från en lista
            {
                if (Random.value < productChanse[i] && hasSpawned == false)   //Om ett tal mellan 0-1 är mindre än produktchansen (sätts i inspektorn) och om inget annat har spawnat
                {
                    SpawnProduct(i);                  //Spawna den produkt som for-satsen var på i listan som mötte kraven (talet var mindre än chansen)
                    hasSpawned = true;                //Spawna inget mer förrän updateCounter möter conditions igen och processen börjar om
                }
            }
            if (!hasSpawned)
                SpawnProduct(0);                    //Om inget i listan spawnade, spawna metallklumpen


            productsSeen++;
            //Debug.Log("Seen products: "+productsSeen);

            if (productsSeen >= unlockedTypes && productsSeen > 1) // && inTheNameOfTheLaw
                changeSpawnStopProducts();
            //inTheNameOfTheLaw=true;
        }

        if (!stayUntilCompleted && productsAreStopped)
        {
            stopCounter++;
            if (stopCounter == 50)
            {
                changeSpawnStopProducts();
                stopCounter = 0;
            }
        }

        if (stayUntilCompleted && productsAreStopped && !spawnFlaskor)
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
                sr.sprite = produktScript.Sprites[type];
                break;
            case 1:
                type++;
                sr.sprite = produktScript.Sprites[type];
                break;
            case 2:
                type++;
                sr.sprite = produktScript.Sprites[type];
                break;
            case 3:
                type++;
                sr.sprite = produktScript.Sprites[type];
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
        stayUntilCompleted = !stayUntilCompleted;
    }

    public void changeSpawnaFlaskor()
    {
        //updateCounter=0;
        //spawnFlaskorJustChanged=1;
        spawnFlaskor = !spawnFlaskor;
    }

    public void setUnlockedTypes(int t)
    {
        unlockedTypes = t;
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
        //QuitWork();
        SceneManager.LoadScene("HUBScene");
        //gmScript.LoadHUB();
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("WorkTutorialScene");
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

    public void LoadResultScreen()
    {
        resultScreen.SetActive(true);
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
        productList.RemoveAt(0);
    }


}
