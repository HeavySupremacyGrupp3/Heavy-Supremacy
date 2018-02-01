using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class angstTextScript : MonoBehaviour {

	public float Angst;
	Stats StatReference;
	private Text t;

	void Start () 
	{
        StatReference = FindObjectOfType<angstStatScript>();
		Angst=StatReference.getAmount();		
		t=GetComponent<Text> ();
	}

	void Update () 
	{
		//StatReference = FindObjectOfType<energyStatScript>();
		Angst = StatReference.getAmount();
		t.text = "Angst: " + Angst;
	}
}