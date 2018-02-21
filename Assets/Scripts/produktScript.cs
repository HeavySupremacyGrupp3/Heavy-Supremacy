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

    private MachineBehaviour mb;

    bool moving=true;
    bool waiting = false;
    bool isDoneMoving = false;

    private GameObject[] checkpoints;
    int checkpointIndex = 0;
	
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
        mb = GameObject.Find("MachineBehaviour").GetComponent<MachineBehaviour>();
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x + mb.spacing / 2f, startPosition.y);
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        checkpointIndex = checkpoints.Length - 1;
    }

    void Update ()
	{
        if (transform.position.x < checkpoints[checkpointIndex].transform.position.x || isDoneMoving)
        {
            transform.Translate(Vector3.right * 3f * Time.deltaTime);
        }
        else if (!waiting)
        {
            //transform.position = endPosition;
            StartCoroutine(StartMovingAfter(2f));
            waiting = true;
        }
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
    private IEnumerator StartMovingAfter(float time)
    {
        yield return new WaitForSeconds(time);
        if (checkpointIndex > 0)
            checkpointIndex--;
        else
            isDoneMoving = true;
        waiting = false;
        //startPosition = transform.position;
        //endPosition = new Vector3(startPosition.x + mb.spacing / 2f, startPosition.y);
    }

}
