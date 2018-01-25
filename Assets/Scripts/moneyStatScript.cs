using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneyStatScript : Stats {

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
		addOrRemoveAmount(50f);
		//amount += 50f;
	}
}
