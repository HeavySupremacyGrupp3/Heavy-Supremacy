using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moneyTextScript : MonoBehaviour {

	public float Money;
	Stats StatReference;
	private Text t;

	void Start () 
	{
		StatReference = FindObjectOfType<energyStatScript>();
		Money=StatReference.getAmount();		
		t=GetComponent<Text> ();
	}

	void Update () 
	{
		Money = StatReference.getAmount();
		t.text = "Money: " + Money;
	}
}
