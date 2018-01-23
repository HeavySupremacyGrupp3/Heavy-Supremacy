using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class happinessTextScript : MonoBehaviour {

	public float Money;
	Stats StatReference;
	private Text t;

	void Start () 
	{
		StatReference=GameObject.Find("happinessObject").GetComponent<happinessStatScript>();
		Money=StatReference.getAmount();		
		t=GetComponent<Text> ();
	}

	void Update () 
	{
		Money = StatReference.getAmount();
		t.text = "Happiness: " + Money;
	}

}