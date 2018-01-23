using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class happinessTextScript : MonoBehaviour {

	public float Happiness;
	Stats StatReference;
	private Text t;

	void Start () 
	{
        StatReference = FindObjectOfType<happinessStatScript>();
		Happiness=StatReference.getAmount();		
		t=GetComponent<Text> ();
	}

	void Update () 
	{
		Happiness = StatReference.getAmount();
		t.text = "Happiness: " + Happiness;
	}
}