using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class produktScript : MonoBehaviour
{

    public delegate void mittEvent();
    public static event mittEvent earnMoney;

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

    private void Start()
    {
        mgm = GameObject.Find("WorkManager").GetComponent<MiniGameManager>();
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x + spacing / 2f, startPosition.y);
        checkpoint = mgm.checkpoint;               //Hämtar checkpoints från MinigameManager
        productList = mgm.productList;              //Hämtar listan av produkter som är spawnade från MiniGameManager       
    }

    void Update()
    {
        if (transform.position.x < checkpoint.transform.position.x && !waiting || reachedCheckpoint && !waiting) //Om produkten inte är framme vid checkpoint, rör på den. Eller om Den har gått förbi check. och inte är waiting
        {
            transform.Translate(Vector3.right * 3f * Time.deltaTime);                      //Rör på produkten
        }
        else if (!waiting)                                                         
        {
            foreach (GameObject product in productList)         //För varje produkt som är ute i scenen (som har lagts i listan i minigamemanager)
            {
                product.GetComponent<produktScript>().Wait();                  //Kör wait på varje produkt i scenen
            }
            //mgm.changeSpawnaFlaskor();
            StartCoroutine(StartMovingAfterCheckpoint(1f));            //En separat coroutine som säger att produkten har gått förbi checkpointen när den har stått där i 2 sek (2f)
        }
    }

    public void Wait()
    {
        waiting = true;
        StartCoroutine(StartMovingAfter(1f));               //Kör igång coroutine som väntar 2 sekunder innan den sätter waiting till false (produkter kan röra sig igen)
    }

    void OnTriggerEnter2D(Collider2D other) //när produkten når slutet och man ska tjäna pengar
    {
        if (other.gameObject.tag == "Box" && gameObject != null)
        {
            if (!Spoiled)
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

    private IEnumerator StartMovingAfter(float time)
    {
        yield return new WaitForSeconds(time);
        waiting = false;
    }

    private IEnumerator StartMovingAfterCheckpoint(float time)
    {
        TransformLooper tl;         //Dålig lösning på att båda rullbanden ska stanna när produkterna gör det (kallas från nuvarande gameobject)
        TransformLooper tl2;
        tl = GameObject.Find("Work_Rullband").GetComponent<TransformLooper>();
        tl2 = GameObject.Find("Work_Rullband2").GetComponent<TransformLooper>();
        tl.StopLoop();
        tl2.StopLoop();
        yield return new WaitForSeconds(time);
        reachedCheckpoint = true;
        tl.StopLoop();
        tl2.StopLoop();
        //mgm.changeSpawnaFlaskor();
    }
}
