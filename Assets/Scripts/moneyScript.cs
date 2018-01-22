using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moneyScript : MonoBehaviour {

	public float Money;
	public Stats StatReference;
	private Text t;

	void Start () 
	{
		DontDestroyOnLoad(gameObject);
		//StatReference.GetComponent<Slider>().enabled=false;
		Money = StatReference.getAmount();
		t=GetComponent<Text> ();
	}

	void Update () 
	{
		t.text = "Money: " + Money;
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
		Money += 50f;
	}

	public void setMoney(float value)
	{
		Money = value;
	}

	public float getMoney()
	{
		return Money;
	}
}
