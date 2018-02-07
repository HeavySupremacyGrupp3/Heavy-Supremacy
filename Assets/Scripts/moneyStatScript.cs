using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneyStatScript : Stats {
	
	
	public float difference;

	public override void initialize()
	{
		thisStat=4;
	}


	void OnEnable() //events
	{
		produktScript.earnMoney +=addMoney;
	}
	
	void OnDisable() //events
	{
		produktScript.earnMoney -=addMoney;
	}	

	public void addMoney()
	{
		addOrRemoveAmount(difference);
		//amount += 50f;
	}
}
