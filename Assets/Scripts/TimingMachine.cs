using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingMachine : TimingSystem {
	
	int timesChanged=0;
	int waitTimer=0;
	
	public delegate void tutorialEvent();
	public static event tutorialEvent productHit;
	public delegate void tutorialCompareEvent(bool b);
	public static event tutorialCompareEvent productCompared;
	
	public machineOutOfRangeDetector MyOutOfRangeDetector;
	//public machineBelowDetectorScript myBelowDetector;
	public int myType;
	
	public string SecondSound;

    void Start()
    {

    }
	public override void SucceedTiming()
    {
		produktScript sc=targets[0].GetComponent<produktScript>();
		
		if(timesChanged==0 && !sc.Spoiled)
		{
			base.SucceedTiming();
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
		Debug.Log("Comparing... ");
		if(myType<sc.type && !sc.Spoiled)
		{
			Debug.Log("Product below can be turned into chickpeas.");
			productCompared(true);
		}
		else
		{
			Debug.Log("NO CHICKPEAS!");
			productCompared(false);
		}
	}
	
	public override void FailTiming()
    {
        base.FailTiming();
    }
	
	private void OnTriggerEnter2D(Collider2D collision) //OnTriggerEnter2D nummer 2
	{
		produktScript sc=collision.GetComponent<produktScript>();
		
		if(myType<sc.type)
		{
			Debug.Log("Produkten behöver inte förvandlas. Skäms på dig!");
			sc.spoil();
			AudioManager.instance.Play("spoilLjud");
			timesChanged=0;
		}
		
		if(myType==sc.type && !sc.Spoiled)
		{
			//Debug.Log("Jag förvandlar.");
			AudioManager.instance.Play("Machine"+myType+"Hit");
			sc.type++;
			collision.GetComponent<SpriteRenderer>().sprite=sc.Sprites[sc.type];
		}
		
		if(myType>sc.type)
		{
			//Debug.Log("Hit kan man flytta förstörelse.");
		}
		
		productHit();
		AudioManager.instance.Play(SecondSound);
	}
	
	private void OnTriggerExit2D(Collider2D collision)
	{
		targets.RemoveAt(0);
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
			sc.spoil();
		}
		timesChanged=0;
	}
}
