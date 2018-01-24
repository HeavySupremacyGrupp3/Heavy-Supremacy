using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingMachine : TimingSystem {
	
	int timesChanged=0;
	int waitTimer=0;
	
	public delegate void mittEvent();
	public static event mittEvent productDetected;
	
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
	
	public override void FailTiming()
    {
        base.FailTiming();
    }
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		timesChanged=0;
		productDetected();
	}
	
	private void OnTriggerExit2D(Collider2D collision)
	{
		if(timesChanged==0)
		{
			produktScript sc=target.GetComponent<produktScript>();
			sc.spoil();
		}
		//timesChanged=0;
	}
}
