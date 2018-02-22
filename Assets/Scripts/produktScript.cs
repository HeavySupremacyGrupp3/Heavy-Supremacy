using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class produktScript : MonoBehaviour
{

    public delegate void mittEvent();
    public static event mittEvent earnMoney;

    public Sprite[] Sprites;
    public Sprite spoilSprite;
    //public int currentStage=0;
    public bool Spoiled = false;

    private Vector3 startPosition;
    private Vector3 endPosition;

    public int type = 0;

    private MachineBehaviour mb;
    private MiniGameManager mgm;

    bool moving = true;
    bool waiting = false;
    bool reachedCheckpoint = false;

    private Transform checkpoint;
    private List<GameObject> productList;

    void OnEnable()
    {
        MiniGameManager.stopProducts += stopAndGo;
    }

    void OnDisable()
    {
        MiniGameManager.stopProducts -= stopAndGo;
    }

    private void Start()
    {
        mb = GameObject.Find("MachineBehaviour").GetComponent<MachineBehaviour>();
        mgm = GameObject.Find("WorkManager").GetComponent<MiniGameManager>();
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x + mb.spacing / 2f, startPosition.y);
        checkpoint = mb.checkpoint;               //Hämtar checkpoints från MachineBehavior
        productList = mgm.productList;              //Hämtar listan av produkter som är spawnade från MiniGameManager
    }

    void Update()
    {
        if (transform.position.x < checkpoint.transform.position.x && !waiting || reachedCheckpoint && !waiting)
        {
            transform.Translate(Vector3.right * 3f * Time.deltaTime);
        }
        else if (!waiting)
        {
            foreach (GameObject product in productList)
            {
                product.GetComponent<produktScript>().Wait();
            }
            StartCoroutine(StartMovingAfterCheckpoint(2f));
        }
    }

    public void Wait()
    {
        waiting = true;
        StartCoroutine(StartMovingAfter(2f));
    }

    void OnTriggerEnter2D(Collider2D other) //kollisioner
    {
        if (other.gameObject.tag == "Box" && gameObject != null)
        {
            if (!Spoiled)
                earnMoney();

            Destroy(gameObject);
            mgm.RemoveFromList();
        }
        //if (other.gameObject.tag == "Maskin")
        //{
        //   mgm = FindObjectOfType<MiniGameManager>();
        // mgm.Collided(gameObject);
        //}

    }

    public void spoil()
    {
        GetComponent<SpriteRenderer>().sprite = spoilSprite;
        Spoiled = true;
    }

    void stopAndGo()
    {
        moving = !moving;

    }

    private IEnumerator StartMovingAfter(float time)
    {
        yield return new WaitForSeconds(time);
        waiting = false;
    }

    private IEnumerator StartMovingAfterCheckpoint(float time)
    {
        yield return new WaitForSeconds(time);
        reachedCheckpoint = true;
    }

}
