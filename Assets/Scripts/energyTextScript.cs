using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class energyTextScript : MonoBehaviour {

	public float energy;
	Stats StatReference;
	private Text t;

	void Start () 
	{
		StatReference = FindObjectOfType<energyStatScript>();
		energy=StatReference.getAmount();		
		t=GetComponent<Text> ();
	}

	void Update () 
	{
		energy = StatReference.getAmount();
		t.text = "Energy: " + energy;
	}
}
