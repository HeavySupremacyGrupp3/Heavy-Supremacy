using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyStatScript : Stats
{
	public override void initialize()
	{
		thisStat=0;
	}
	
	void OnEnable() //events
	{
		GameManager.sleep +=addEnergy;
	}
	
	void OnDisable() //events
	{
		GameManager.sleep -=addEnergy;
	}	

	public void addEnergy()
	{
		addOrRemoveAmount(0.5f);
		//amount += 50f;
	}
}
