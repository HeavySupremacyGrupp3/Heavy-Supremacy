using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class produktScript : MonoBehaviour {

	public delegate void mittEvent();
	public static event mittEvent earnMoney;
	
	public Sprite[] Sprites;
	public Sprite spoilSprite;
	//public int currentStage=0;
	public bool Spoiled=false;

    private Vector3 startPosition;
    private Vector3 endPosition;

    public int type = 0;

    public MiniGameManager mgm;
    private MachineBehaviour mb;


    bool moving=true;
	
	void OnEnable()
	{
		MiniGameManager.stopProducts +=stopAndGo;
	}
	
	void OnDisable()
	{
		MiniGameManager.stopProducts -=stopAndGo;
	}

    private void Start()
    {
        MiniGameManager mgm = GetComponent<MiniGameManager>();
        MachineBehaviour mb = GetComponent<MachineBehaviour>();
        startPosition = transform.position;
        //endPosition = new Vector3(startPosition.x + mb.spacing, startPosition.y);
        
    }

    void Update ()
	{
		if(moving)
			transform.Translate(Vector3.right * 3f*Time.deltaTime);
       // StartCoroutine(Stop(moving));
    }
	
	void OnTriggerEnter2D(Collider2D other) //kollisioner
	{
		if(other.gameObject.tag=="Box")
		{
			if(!Spoiled)
				earnMoney();
			
			Destroy(gameObject);
		}
        //if (other.gameObject.tag == "Maskin")
        //{
         //   mgm = FindObjectOfType<MiniGameManager>();
           // mgm.Collided(gameObject);
        //}

    }
	
	public void spoil()
	{
		GetComponent<SpriteRenderer>().sprite=spoilSprite;
		Spoiled=true;
	}
	
	void stopAndGo()
	{
        moving =!moving;

	}

   /* private void Stop()
    {
        if (transform.position.x >= endPosition.x && transform.position.x <= endPosition.x + 0.1)
        {
            for (float i = 0; i <= 2; i += Time.deltaTime)
            {
                Debug.Log("stopped");
                moving = false;
            }
        }
        Debug.Log("moves");
        moving = true;
    }*/
    
        //this is supposed to work and stop the producst and stuff but moving is weird...hehe...(omg!)
    private IEnumerator Stop(bool moving)
    {
        if (moving)
        {
            Debug.Log("Started coroutine");
            if (transform.position.x >= endPosition.x  && transform.position.x <= endPosition.x + 0.1)
            {
                Debug.Log("Reached stop.x");
                moving = false;
                yield return new WaitForSeconds(2f);
                moving = true;
            }
        }
    }

}
