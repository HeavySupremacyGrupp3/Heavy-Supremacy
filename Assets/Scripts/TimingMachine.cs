using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingMachine : TimingSystem {
	
	int timesChanged=0;
	int waitTimer=0;
	
	//public delegate void musicEvent(string s);
	//public static event musicEvent productDetected;
	
	public delegate void tutorialEvent();
	public static event tutorialEvent productHit;
	
	public machineOutOfRangeDetector MyOutOfRangeDetector;
	public machineBelowDetectorScript myBelowDetector;
	public int myType;
	
	public string SecondSound;
	
	public override void SucceedTiming()
    {
		produktScript sc=targets[0].GetComponent<produktScript>();
		
		if(timesChanged==0 && !sc.Spoiled)
		{
			base.SucceedTiming();			
			//sc.currentStage++;
			
			//if(sc.currentStage<sc.Sprites.Length)
				//target.GetComponent<SpriteRenderer>().sprite=sc.Sprites[sc.currentStage];
			
			timesChanged++;			
		}		
    }
	
	void OnEnable()
	{
		MyOutOfRangeDetector.productToSpoilDetected += spoilProducts;
		//myBelowDetector.productBelowDetected += compareTypes;
	}
	
	void OnDisable()
	{
		MyOutOfRangeDetector.productToSpoilDetected -= spoilProducts;
		//myBelowDetector.productBelowDetected -= compareTypes;
	}
	
	void compareTypes(GameObject t)
	{
		produktScript sc=t.GetComponent<produktScript>();
		Debug.Log("Product below");
		if(myType<sc.type)
		{
			Debug.Log("Product below hit");
			//productHit();
		}
	}
	
	public override void FailTiming()
    {
        base.FailTiming();
    }
	
	private void OnTriggerEnter2D(Collider2D collision) //OnTriggerEnter2D nummer 2
	{
		produktScript sc=collision.GetComponent<produktScript>();
		
		//Debug.Log("Jag tar emot produkter av typ: "+myType+" och förvandlar dom till: "+(myType+1));
		if(myType<sc.type)
		{
			//FindObjectOfType<MusicManager>().Play("Arm1Sound");
			Debug.Log("Produkten behöver inte förvandlas. Skäms på dig!");
			sc.spoil();
			//productDetected("spoilLjud");
			AudioManager.instance.Play("spoilLjud");
			//targets.RemoveAt(0);
			timesChanged=0;
		}
		
		if(myType==sc.type && !sc.Spoiled)
		{
			//Debug.Log("Jag förvandlar.");
			sc.type++;
			collision.GetComponent<SpriteRenderer>().sprite=sc.Sprites[sc.type];
		}
		
		if(myType>sc.type)
		{
			//Debug.Log("Hit kan man flytta förstörelse.");
		}
		
		productHit();
		AudioManager.instance.Play(SecondSound);
		//productDetected(SecondSound);
	}
	
	private void OnTriggerExit2D(Collider2D collision)
	{
		targets.RemoveAt(0);
		Debug.Log(targets.Count);
	}
	
	private void old2DCollider(Collider2D collision)
	{
		
		produktScript sc=collision.GetComponent<produktScript>();
        //MiniGameManager mgm = target.GetComponent<MiniGameManager>();

        if (timesChanged==0 && !sc.Spoiled)
		{
			//base.SucceedTiming();
          
			//sc.currentStage++;
			
			//if(sc.currentStage<sc.Sprites.Length)
				//collision.GetComponent<SpriteRenderer>().sprite=sc.Sprites[sc.currentStage];
			
			timesChanged++;			
		}						
		//timesChanged=0;
		//productDetected();
	}
	
	private void spoilProducts(GameObject ta)
	{		
		produktScript sc=ta.GetComponent<produktScript>();
		
		if(myType==sc.type)
		{		
			//Debug.Log("typer: "+myType+" "+sc.type);
			sc.spoil();
		}
		//targets.RemoveAt(0);
		timesChanged=0;
	}
}
