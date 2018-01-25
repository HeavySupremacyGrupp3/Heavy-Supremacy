using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingMachine : TimingSystem {
	
	int timesChanged=0;
	int waitTimer=0;
	
	public delegate void mittEvent();
	public static event mittEvent productDetected;
	public machineOutOfRangeDetector MyOutOfRangeDetector;
	
	public override void SucceedTiming()
    {
		produktScript sc=target.GetComponent<produktScript>();
		
		if(timesChanged==0 && !sc.Spoiled)
		{
			base.SucceedTiming();			
			sc.currentStage++;
			
			if(sc.currentStage<sc.Sprites.Length)
				target.GetComponent<SpriteRenderer>().sprite=sc.Sprites[sc.currentStage];
			
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
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		
		produktScript sc=collision.GetComponent<produktScript>();
		
		if(timesChanged==0 && !sc.Spoiled)
		{
			base.SucceedTiming();			
			sc.currentStage++;
			
			if(sc.currentStage<sc.Sprites.Length)
				collision.GetComponent<SpriteRenderer>().sprite=sc.Sprites[sc.currentStage];
			
			timesChanged++;			
		}		
				
		//timesChanged=0;
		productDetected();
	}
	
	private void spoilProducts(GameObject ta)
	{
		Debug.Log("times changed "+timesChanged);
		if(timesChanged==0)
		{
			target=ta;
			produktScript sc=target.GetComponent<produktScript>();
			sc.spoil();
		}
		timesChanged=0;
	}
}
