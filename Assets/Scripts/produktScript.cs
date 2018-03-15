using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class produktScript : MonoBehaviour
{

    public delegate void mittEvent();
    public static event mittEvent earnMoney;
	public static event mittEvent collidedWithBox;

    public Sprite[] Sprites;	
	
	public int type = 0;
	public float spacing;
	
    public bool Spoiled = false;
    
    bool waiting = false;
    bool reachedCheckpoint = false;   
	
	MiniGameManager mgm;
    Transform checkpoint;

    private void Start()
    {
        mgm = GameObject.Find("WorkManager").GetComponent<MiniGameManager>();
        checkpoint = mgm.checkpoint;               //Hämtar checkpoints från MinigameManager     
    }
	
	void OnEnable()
	{
		MiniGameManager.stopEverything += switchWaiting;
		waiting = false;
	}
	
	void OnDisable()
	{
		MiniGameManager.stopEverything -= switchWaiting;
	}
	
	void switchWaiting()
	{
		waiting=!waiting;
	}

    void Update()
    {
        if (!waiting) //Om produkten inte är framme vid checkpoint, rör på den. Eller om Den har gått förbi check. och inte är waiting
        {
            transform.Translate(Vector3.right * 3f * Time.deltaTime);                      //Rör på produkten
        }
		
		if(transform.position.x > checkpoint.transform.position.x && !reachedCheckpoint)
		{
			reachedCheckpoint=true;
		}
    }

    void OnTriggerEnter2D(Collider2D other) //när produkten når slutet och man ska tjäna pengar
    {
        if (other.gameObject.tag == "Box" && gameObject != null)
        {
			collidedWithBox();
            if(!Spoiled && type==3)
				earnMoney();

            mgm.RemoveFromList();
            Destroy(gameObject);
        }
    }

    public void spoil() //förstör produkten
    {
        Spoiled = true;
    }

    /*private IEnumerator StartMovingAfter(float time)
    {
		timeSinceCoroutineStarted = 0;
		while (timeSinceCoroutineStarted <= time)
		{
			yield return null;
			timeSinceCoroutineStarted += Time.deltaTime;
		}
        waiting = false;
    }

    private IEnumerator StartMovingAfterCheckpoint(float time)
    {
        stopProducts();         //Event som stoppar rullband och kugghjul
        yield return new WaitForSeconds(time);
        reachedCheckpoint = true;
        stopProducts();    //Samma event startar det igen (efter 2 sekunder)
    }
	
	void omRörSig()
	{
		if (!waiting)                                                         
        {
			//Wait();	
            //StartCoroutine(StartMovingAfterCheckpoint(1f));            //En separat coroutine som säger att produkten har gått förbi checkpointen när den har stått där i 2 sek (2f)
        }
	}
	
	public void Wait()
    {
        waiting = true;
        StartCoroutine(StartMovingAfter(1f));               //Kör igång coroutine som väntar 2 sekunder innan den sätter waiting till false (produkter kan röra sig igen)
    }
	*/
}
