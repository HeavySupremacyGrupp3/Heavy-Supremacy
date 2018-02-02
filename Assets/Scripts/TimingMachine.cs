using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingMachine : TimingSystem {
	
	int timesChanged=0;
	int waitTimer=0;
	
	public delegate void mittEvent();
	public static event mittEvent productDetected;
	public machineOutOfRangeDetector MyOutOfRangeDetector;
	public int myType;
	
	public override void SucceedTiming()
    {
		produktScript sc=targets[0].GetComponent<produktScript>();
		
		if(timesChanged==0 && !sc.Spoiled)
		{
			base.SucceedTiming();			
			sc.currentStage++;
			
			//if(sc.currentStage<sc.Sprites.Length)
				//target.GetComponent<SpriteRenderer>().sprite=sc.Sprites[sc.currentStage];
			
			timesChanged++;			
		}		
    }
	
	void OnEnable()
	{
		MyOutOfRangeDetector.productToSpoilDetected += spoilProducts;
	}
	
	void OnDisable()
	{
		MyOutOfRangeDetector.productToSpoilDetected -= spoilProducts;
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
			FindObjectOfType<MusicManager>().Play("Arm1Sound");
			Debug.Log("Produkten behöver inte förvandlas. Skäms på dig!");
			sc.spoil();
			targets.RemoveAt(0);
			timesChanged=0;
		}
		
		if(myType==sc.type)
		{
			//Debug.Log("Jag förvandlar.");
			sc.type++;
			collision.GetComponent<SpriteRenderer>().sprite=sc.Sprites[sc.type];
		}
		
		if(myType>sc.type)
		{
			//Debug.Log("Hit kan man flytta förstörelse.");
		}
		
		productDetected();
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
		productDetected();
	}
	
	private void spoilProducts(GameObject ta)
	{		
		produktScript sc=ta.GetComponent<produktScript>();
		
		if(myType==sc.type)
		{		
			//Debug.Log("typer: "+myType+" "+sc.type);
			sc.spoil();
		}
		targets.RemoveAt(0);
		timesChanged=0;
	}
}
