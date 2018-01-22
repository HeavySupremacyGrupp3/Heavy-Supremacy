using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneyStatScript : Stats {

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
