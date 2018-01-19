using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moneyScript : MonoBehaviour {

	public int Money;
	private Text t;

	void Start () 
	{
		Money = 0;
		t=GetComponent<Text> ();
	}

	void Update () 
	{
		t.text = "Money: " + Money;
	}

	public void addMoney(int value)
	{
		Money += value;
	}

	public void setMoney(int value)
	{
		Money = value;
	}

	public int getMoney()
	{
		return Money;
	}
}
