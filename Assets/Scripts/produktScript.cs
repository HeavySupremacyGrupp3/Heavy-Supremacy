using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class produktScript : MonoBehaviour
{

    public delegate void mittEvent();
    public static event mittEvent earnMoney;
	public static event mittEvent collidedWithBox;	
	public static event mittEvent JustReachedCheckpoint;

    public Sprite[] Sprites;
    public Sprite spoilSprite;   	
	
	public int type = 0;
	public float spacing;
	
    public bool Spoiled = false;

    bool moving = true;
    bool waiting = false;
    bool reachedCheckpoint = false;   
	
	Vector3 startPosition;
    Vector3 endPosition;
	
	MiniGameManager mgm;
    Transform checkpoint;	
    List<GameObject> productList;

    public delegate void Event();
    public static event Event stopProducts;

    private moneyStatScript moneyStats;

    private void Start()
    {
        mgm = GameObject.Find("WorkManager").GetComponent<MiniGameManager>();
        moneyStats = GameObject.Find("moneyObject").GetComponent<moneyStatScript>();
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x + spacing / 2f, startPosition.y);
        checkpoint = mgm.checkpoint;               //Hämtar checkpoints från MinigameManager
        productList = mgm.productList;              //Hämtar listan av produkter som är spawnade från MiniGameManager       
    }
	
	void OnEnable()
	{
		MiniGameManager.stopEverything += switchWaiting;		
	}
	
	void OnDisable()
	{
		MiniGameManager.stopEverything -= switchWaiting;
	}
	
	void switchWaiting()
	{
		//omRörSig();
		waiting=!waiting;
	}

    void Update()
    {
        if (!waiting) //Om produkten inte är framme vid checkpoint, rör på den. Eller om Den har gått förbi check. och inte är waiting
        {
            transform.Translate(Vector3.right * 3f * Time.deltaTime);                      //Rör på produkten
        }
		
		if(!reachedCheckpoint) //transform.position.x > checkpoint.transform.position.x && 
		{
			reachedCheckpoint=true;
			JustReachedCheckpoint();
		}
    }
	
	void omRörSig()
	{
		if (!waiting)                                                         
        {     			
			//Wait();	
            //StartCoroutine(StartMovingAfterCheckpoint(1f));            //En separat coroutine som säger att produkten har gått förbi checkpointen när den har stått där i 2 sek (2f)
        }
	}
	
    void OnTriggerEnter2D(Collider2D other) //när produkten når slutet och man ska tjäna pengar
    {
        if (other.gameObject.tag == "Box" && gameObject != null)
        {
			collidedWithBox();
            if(!Spoiled && type==3)
				earnMoney();

            Destroy(gameObject);
            mgm.RemoveFromList();
        }
    }

    public void spoil() //förstör produkten
    {
        GetComponent<SpriteRenderer>().sprite = spoilSprite;
        Spoiled = true;
    }	
	
}
