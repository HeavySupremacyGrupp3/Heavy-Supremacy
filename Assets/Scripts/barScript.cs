using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barScript : MonoBehaviour {

	public Slider progressSlider;
	
	Stats StatReference;
	
	float amount;

	void Update ()
	{
		StatReference=GameObject.Find("happinessObject").GetComponent<happinessStatScript>();
		amount=StatReference.getAmount();		
		progressSlider.value=amount;
	}
}
