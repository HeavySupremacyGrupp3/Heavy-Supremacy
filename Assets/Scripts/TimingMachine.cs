using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingMachine : TimingSystem {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void SucceedTiming()
    {
		base.SucceedTiming();
		produktScript sc=target.GetComponent<produktScript>();
		sc.currentStage++;
		target.GetComponent<SpriteRenderer>().sprite=sc.Sprites[sc.currentStage];		
    }
	
	public override void FailTiming()
    {
        base.FailTiming();
		produktScript sc=target.GetComponent<produktScript>();
		sc.spoil();
    }

}
